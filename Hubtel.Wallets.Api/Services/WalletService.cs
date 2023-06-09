﻿using AutoMapper;
using FluentValidation;
using Hubtel.Wallets.Api.Constants;
using Hubtel.Wallets.Api.DataAccess;
using Hubtel.Wallets.Api.Dtos;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hubtel.Wallets.Api.Services
{
    public class WalletService : IWalletService
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<WalletDto> _validator;
        private readonly IMapper _mapper;
        private readonly IWalletSecurity _walletSecurity;

        public WalletService(ApplicationDbContext context, IValidator<WalletDto> validator, IMapper mapper, IWalletSecurity walletSecurity)
        {
            _context = context;
            _validator = validator;
            _mapper = mapper;
            _walletSecurity = walletSecurity;
        }

        private const int _walletLimit = 5;

        public async Task<BaseWalletResponseDto> AddWalletAsync(WalletDto walletDto)
        {
            try
            {
                var response = new BaseWalletResponseDto();

                if (walletDto.Type == WalletTypeConstants.Card)
                {
                    var cardAccountNumberExists = await _context.Wallets.AnyAsync(p => p.AccountNumber == _walletSecurity.Encrypt(walletDto.AccountNumber));
                    if (cardAccountNumberExists)
                    {
                        response.Success = false;
                        response.Message = $"The card wallet account {walletDto.AccountNumber} already exists.";

                        return response;
                    }
                }
                else if (walletDto.Type == WalletTypeConstants.Momo)
                {
                    var momoAccountNumberExists = await _context.Wallets.AnyAsync(p => p.AccountNumber == walletDto.AccountNumber);
                    if (momoAccountNumberExists)
                    {
                        response.Success = false;
                        response.Message = $"The momo wallet account {walletDto.AccountNumber} already exists.";

                        return response;
                    }
                }

                var validationResults = _validator.Validate(walletDto);
                if (!validationResults.IsValid)
                {
                    response.Success = false;
                    response.Message = "One or more validations failed";
                    response.Errors = validationResults.Errors.Select(p => p.ErrorMessage).ToList();

                    return response;
                }


                var walletLimitExceeded = await _context.Wallets.CountAsync(p => p.Owner == walletDto.Owner) == _walletLimit;
                if (walletLimitExceeded)
                {
                    response.Success = false;
                    response.Message = $"A customer cannot have more than {_walletLimit} wallets";

                    return response;
                }                

                if (walletDto.Type == WalletTypeConstants.Card)
                {
                    walletDto.AccountNumber = _walletSecurity.Encrypt(walletDto.AccountNumber);
                }

                var wallet = _mapper.Map<Wallet>(walletDto);
                _context.Add(wallet);
                await _context.SaveChangesAsync();

                response.Id = wallet.Id;
                response.Success = true;
                response.Message = "Wallet was added successfully";

                return response;
            }
            catch (Exception ex)
            {
                return new BaseWalletResponseDto()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<IReadOnlyList<WalletDto>> GetAllWalletAsync()
        {
            var walletsFromDatabase = await _context.Wallets.AsNoTracking().ToListAsync();
            if (walletsFromDatabase is null)
            {
                return null;
            }
            var wallets = _mapper.Map<IReadOnlyList<WalletDto>>(walletsFromDatabase);
            foreach (var wallet in wallets)
            {
                wallet.AccountNumber = _walletSecurity.Decrypt(wallet.AccountNumber);
            }
            return wallets;
        }

        public async Task<WalletDto> GetWalletAsync(int walletId)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet is null)
            {
                return null;
            }

            var walletDto = _mapper.Map<WalletDto>(wallet);
            walletDto.AccountNumber = _walletSecurity.Decrypt(walletDto.AccountNumber);
            return walletDto;
        }

        public async Task<BaseWalletResponseDto> RemoveWalletAsync(int walletId)
        {
            var response = new BaseWalletResponseDto();
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet is null)
            {
                response.Success = false;
                response.Message = "Wallet does not exist";
                return response;
            }
            _context.Remove(wallet);
            await _context.SaveChangesAsync();

            
            response.Success = true;
            response.Message = "Wallet removed successfully";

            return response;
        }

    }
}
