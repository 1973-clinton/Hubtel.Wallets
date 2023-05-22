using FluentValidation;
using Hubtel.Wallets.Api.Dtos;
using Hubtel.Wallets.Api.Models;

namespace Hubtel.Wallets.Api.Validations
{
    public class WalletValidator : AbstractValidator<WalletDto>
    {
        public WalletValidator()
        {
            RuleFor(wallet => wallet.Name).NotEmpty().MaximumLength(100).Matches(@"^[a-zA-Z]");

            RuleFor(wallet => wallet.Owner).NotEmpty().MaximumLength(10).Matches(@"^[0-9]");

            RuleFor(wallet => wallet.AccountScheme).NotEmpty();

            RuleFor(wallet => wallet.Type).NotEmpty().MaximumLength(4);

            // validates account number if it starts with phone number code '0', else it validates it as credit card
            When(wallet => wallet.AccountNumber.StartsWith("0"), () =>
            {
                RuleFor(wallet => wallet.AccountNumber).NotEmpty().MaximumLength(10).Matches(@"^[0-9]");               
            })
            .Otherwise(() =>
            {
                RuleFor(wallet => wallet.AccountNumber).NotEmpty().CreditCard().Matches(@"^[0-9]");
            });

        }
    }
}
