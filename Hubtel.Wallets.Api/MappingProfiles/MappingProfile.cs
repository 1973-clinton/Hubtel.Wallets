using AutoMapper;
using Hubtel.Wallets.Api.Dtos;
using Hubtel.Wallets.Api.Models;

namespace Hubtel.Wallets.Api.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WalletDto, Wallet>().ReverseMap();
        }
    }
}
