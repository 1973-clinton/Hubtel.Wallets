using AutoMapper;
using FluentValidation;
using Hubtel.Wallets.Api.DataAccess;
using Hubtel.Wallets.Api.Dtos;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Hubtel.Wallets.Api.Services
{
    public class WalletService : IWalletService
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<WalletDto> _validator;
        private readonly IMapper _mapper;

        public WalletService(ApplicationDbContext context, IValidator<WalletDto> validator, IMapper mapper)
        {
            _context = context;
            _validator = validator;
            _mapper = mapper;
        }

        private const int _walletLimit = 5;

        public async Task<BaseWalletResponseDto> AddWalletAsync(WalletDto walletDto)
        {
            
            var response = new BaseWalletResponseDto();

            var validationResults = _validator.Validate(walletDto);
            if (!validationResults.IsValid)
            {
                response.Success = false;
                response.Message = "One or more validations failed";
                response.Errors = validationResults.Errors.Select(p => p.ErrorMessage).ToList();

                return response;
            }

            
            if(_context.Wallets.Count(p => p.Owner == walletDto.Owner) == _walletLimit)
            {
                response.Success = false;
                response.Message = $"A customer cannot have more than {_walletLimit} wallets";

                return response;
            }

            var wallet = _mapper.Map<Wallet>(walletDto);
            _context.Add(wallet);
            await _context.SaveChangesAsync();

            response.Id = wallet.Id;
            response.Success = true;
            response.Message = "Wallet was added successfully";

            return response;
        }

        public async Task<IReadOnlyList<WalletDto>> GetAllWalletAsync()
        {
            var wallets = await _context.Wallets.AsNoTracking().ToListAsync();
            if (wallets is null)
            {
                return null;
            }
            var response = _mapper.Map<IReadOnlyList<WalletDto>>(wallets);

            return response;
        }

        public async Task<WalletDto> GetWalletAsync(int walletId)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet is null)
            {
                return null;
            }

            var walletDto = _mapper.Map<WalletDto>(wallet);

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
