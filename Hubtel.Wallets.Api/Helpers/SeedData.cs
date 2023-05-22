using Hubtel.Wallets.Api.Constants;
using Hubtel.Wallets.Api.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace Hubtel.Wallets.Api.Helpers
{
    public static class SeedData
    {
        public static void SeedAdminUser(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync(CustomIdentityConstants.UserEmail).Result == null) 
            {
                var user = new ApplicationUser
                {
                    Email = CustomIdentityConstants.UserEmail,
                    UserName = CustomIdentityConstants.UserEmail,
                    LockoutEnabled = false,
                    PhoneNumber = "0202437997"
                };

                var result = userManager.CreateAsync(user, CustomIdentityConstants.Password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, AuthorizationContants.Admin).Wait();
                }
            }
        }
    }
}
