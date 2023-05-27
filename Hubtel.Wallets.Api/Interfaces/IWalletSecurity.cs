namespace Hubtel.Wallets.Api.Interfaces
{
    public interface IWalletSecurity
    {
        public string Encrypt(string plainText);
        public string Decrypt(string encryptedText);
    }
}
