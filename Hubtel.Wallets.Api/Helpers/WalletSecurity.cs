using Hubtel.Wallets.Api.Configurations;
using Hubtel.Wallets.Api.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Hubtel.Wallets.Api.Helpers
{
    public class WalletSecurity : IWalletSecurity
    {
        private readonly IDataProtector _protector;
        //private readonly PurposeString _purposeString;
        private readonly IConfiguration _configuration;

        public WalletSecurity(IDataProtectionProvider provider, IOptions<PurposeString> purposeString, IConfiguration configuration)
        {
            //_purposeString = purposeString.Value;
            _configuration = configuration;
            _protector = provider.CreateProtector(_configuration.GetValue<string>("PurposeString:AccountNumberPurposeString", "&Pmrpo6eStr!ngAcc0unt!Nuwber"));
            
        }
        public string Decrypt(string encryptedText)
        {
            return _protector.Unprotect(encryptedText);
        }

        public string Encrypt(string plainText)
        {
           return _protector.Protect(plainText.Replace(" ", "")[..6]);
        }
    }
}
