using System.Collections.Generic;

namespace Hubtel.Wallets.Api.Dtos
{
    public class BaseWalletResponseDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public bool Success { get; set; } = true;
    }
}
