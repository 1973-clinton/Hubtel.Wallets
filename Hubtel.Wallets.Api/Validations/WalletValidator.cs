using FluentValidation;
using Hubtel.Wallets.Api.Constants;
using Hubtel.Wallets.Api.Dtos;
using System.Collections.Generic;
using System.Linq;

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

            RuleFor(wallet => wallet.Name).NotEmpty().MaximumLength(100).Matches(ValidationConstants.MustBeAplhabetsOnly);

            RuleFor(wallet => wallet.Owner).NotEmpty().MaximumLength(10).Matches(ValidationConstants.MustBeNumericOnly);

            RuleFor(wallet => wallet.AccountScheme).NotEmpty().Must(scheme => accountSchemes.Contains(scheme));

            RuleFor(wallet => wallet.Type).NotEmpty().MaximumLength(4).Must(walletType => walletTypes.Contains(walletType));
            
            // validates account number if its of 'momo' type, else it validates it as credit card
            When(wallet => wallet.Type == WalletTypeConstants.Momo, () =>
            {
                // validating using only the first digit (0) causes data integrity issue. In this case its best to use the acctNo type and first digit (0)
                RuleFor(wallet => wallet.AccountNumber).NotEmpty().MaximumLength(10).Matches(ValidationConstants.MustBeNumericOnly).Must(p => p.StartsWith("0")).WithMessage("Account type does not corrrespond with account number. Momo account number must start with '0'");             
            })
            .Otherwise(() =>
            {
                RuleFor(wallet => wallet.AccountNumber).NotEmpty().CreditCard().Matches(ValidationConstants.MustBeNumericOnly);
                RuleFor(wallet => wallet.AccountScheme).NotEmpty().Must(scheme => accountSchemes.Contains(scheme));
            });

        }

        
    }
}
