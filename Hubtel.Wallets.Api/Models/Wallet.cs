using System;

namespace Hubtel.Wallets.Api.Models
{
    public class Wallet
    {
        public Wallet()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string AccountNumber { get; set; }
        public string AccountScheme { get; set; }
        public string Owner { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
