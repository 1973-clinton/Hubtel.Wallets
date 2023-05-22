using Hubtel.Wallets.Api.Constants;
using Hubtel.Wallets.Api.Dtos;
using Hubtel.Wallets.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Hubtel.Wallets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AuthorizationContants.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public AdminController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("/wallet")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BaseWalletResponseDto))]
        public async Task<IActionResult> AddWallet([FromBody] WalletDto walletDto)
        {
            var response = await _walletService.AddWalletAsync(walletDto);

            if (!response.Success)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            return StatusCode(StatusCodes.Status201Created, response);
        }

        
        [HttpGet("/wallet/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WalletDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWallet(int id)
        {
            var response = await _walletService.GetWalletAsync(id);
            if (response is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        
        [HttpGet("/wallet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<WalletDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllWallets()
        {
            var response = await _walletService.GetAllWalletAsync();
            if (response is null)
            {
               return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete("/wallet/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseWalletResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            var response = await _walletService.RemoveWalletAsync(id);
            if (!response.Success)
            {
                return StatusCode(StatusCodes.Status404NotFound, response);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
