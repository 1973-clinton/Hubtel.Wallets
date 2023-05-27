using FluentValidation;
using Hubtel.Wallets.Api.Constants;
using Hubtel.Wallets.Api.Dtos;
using System.Collections.Generic;

namespace Hubtel.Wallets.Api.Validations
{
    public class WalletValidator : AbstractValidator<WalletDto>
    {

        public WalletValidator()
        {
            var walletTypes = new List<string>()
            {
                WalletTypeConstants.Card,
                WalletTypeConstants.Momo
            };

            var accountSchemes = new List<string>()
            {
                AccountSchemeConstants.MasterCard,
                AccountSchemeConstants.Mtn,
                AccountSchemeConstants.Visa,
                AccountSchemeConstants.Vodafone,
                AccountSchemeConstants.AirtelTigo
            };

            var momoAccountScheme = new List<string>()
            {
                MomoAccountSchemeConstants.Vodafone,
                MomoAccountSchemeConstants.Mtn,
                MomoAccountSchemeConstants.AirtelTigo
            };

            var cardAccountScheme = new List<string>()
            {
                CardAccountSchemeConstants.MasterCard,
                CardAccountSchemeConstants.Visa
            };


            RuleFor(wallet => wallet.Name).NotEmpty().MaximumLength(100).Matches(ValidationConstants.MustBeAplhabetsOnly);

            RuleFor(wallet => wallet.Owner).NotEmpty().MaximumLength(10).Matches(ValidationConstants.MustBeNumericOnly);

            RuleFor(wallet => wallet.AccountScheme).NotEmpty().Must(scheme => accountSchemes.Contains(scheme));

            RuleFor(wallet => wallet.Type).NotEmpty().MaximumLength(4).Must(walletType => walletTypes.Contains(walletType)).WithMessage($"Wallet type must be {WalletTypeConstants.Momo} or {WalletTypeConstants.Card}");

            // validates account number if its of 'momo' type, else it validates it as credit card
            When(wallet => wallet.Type == WalletTypeConstants.Momo, () =>
            {
                // validating using only the first digit (0) causes data integrity issue. In this case its best to use the acctNo type and first digit (0)
                RuleFor(wallet => wallet.AccountNumber).NotEmpty().MaximumLength(10).Matches(ValidationConstants.MustBeNumericOnly).Must(p => p.StartsWith("0")).WithMessage("Account type does not corrrespond with account number. Momo account number must start with '0'");
                RuleFor(wallet => wallet.AccountScheme).NotEmpty().MaximumLength(10).Must(scheme => momoAccountScheme.Contains(scheme)).WithMessage($"Momo account scheme must be {MomoAccountSchemeConstants.AirtelTigo}, {MomoAccountSchemeConstants.Mtn} or {MomoAccountSchemeConstants.Vodafone}"); ;
            });

            When(wallet => wallet.Type == WalletTypeConstants.Card, () =>
            {
                RuleFor(wallet => wallet.AccountNumber).NotEmpty().CreditCard().Matches(ValidationConstants.MustBeNumericOnly);
                RuleFor(wallet => wallet.AccountScheme).NotEmpty().Must(scheme => cardAccountScheme.Contains(scheme)).WithMessage("Card account scheme must be visa or mastercard");

                When(wallet => wallet.AccountScheme == AccountSchemeConstants.Visa, () =>
                {
                    RuleFor(wallet => wallet.AccountNumber).Must(p => p.StartsWith("4")).WithMessage("Invalid visa acount");
                })
                .Otherwise(() =>
                {
                    RuleFor(wallet => wallet.AccountNumber).Must(p => p.StartsWith("2") || p.StartsWith("5")).WithMessage("Invalid mastercard acount");
                });
            });

        }

        
    }
}
