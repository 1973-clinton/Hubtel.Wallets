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

            RuleFor(wallet => wallet.AccountScheme).NotEmpty().Must(p => accountSchemes.Contains(p));

            RuleFor(wallet => wallet.Type).NotEmpty().MaximumLength(4).Must(p => walletTypes.Contains(p));
            
            // validates account number if it starts with phone number code '0', else it validates it as credit card
            When(wallet => wallet.AccountNumber.StartsWith("0"), () =>
            {
                RuleFor(wallet => wallet.AccountNumber).NotEmpty().MaximumLength(10).Matches(ValidationConstants.MustBeNumericOnly);               
            })
            .Otherwise(() =>
            {
                RuleFor(wallet => wallet.AccountNumber).NotEmpty().CreditCard().Matches(ValidationConstants.MustBeNumericOnly);
            });

        }

        
    }
}
