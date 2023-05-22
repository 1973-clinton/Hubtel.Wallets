using System.ComponentModel.DataAnnotations;

namespace Hubtel.Wallets.Api.Dtos
{
    public class AuthRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
