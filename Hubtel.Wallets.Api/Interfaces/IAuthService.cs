using Hubtel.Wallets.Api.Dtos;
using System.Threading.Tasks;

namespace Hubtel.Wallets.Api.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> GenerateTokenAsync(AuthRequestDto authRequest);
    }
}
