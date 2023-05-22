using Hubtel.Wallets.Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hubtel.Wallets.Api.Interfaces
{
    public interface IWalletService
    {
        Task<BaseWalletResponseDto> AddWalletAsync(WalletDto walletDto);
        Task<BaseWalletResponseDto> RemoveWalletAsync(int walletId);
        Task<IReadOnlyList<WalletDto>> GetAllWalletAsync();
        Task<WalletDto> GetWalletAsync(int walletId);
    }
}
