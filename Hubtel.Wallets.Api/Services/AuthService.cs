using Hubtel.Wallets.Api.Configurations;
using Hubtel.Wallets.Api.Dtos;
using Hubtel.Wallets.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Linq;
using Hubtel.Wallets.Api.Interfaces;
using Microsoft.Extensions.Options;

namespace Hubtel.Wallets.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Jwt _jwt;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<Jwt> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }
        public async Task<AuthResponseDto> GenerateTokenAsync(AuthRequestDto authRequest)
        {
            var authResponse = new AuthResponseDto();
            var user = await _userManager.FindByEmailAsync(authRequest.UserName);
            if (user == null)
            {
                authResponse.Message = $"User does not exist";
                return authResponse;
            }
            if (await _userManager.CheckPasswordAsync(user, authRequest.Password))
            {
                JwtSecurityToken jwtSecurityToken = await CreateJwtTokenAsync(user);
                authResponse.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authResponse.Message = "Success";

                return authResponse;
            }

            authResponse.Message = $"Incorrect credentials for user {user.Email}.";

            return authResponse;
        }

        private async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
