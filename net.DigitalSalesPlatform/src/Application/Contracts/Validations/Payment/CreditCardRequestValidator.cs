using Application.Contracts.Constants.Payment;
using FluentValidation;
using Infrastructure.Payment.Requests;

namespace Application.Contracts.Validations.Payment;

public class CreditCardRequestValidator : AbstractValidator<CreditCardRequest>
{
    public CreditCardRequestValidator()
    {
        RuleFor(x=>x.CreditCardNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage(PaymentValidationMessages.CreditCardNumberRequired);
        
        RuleFor(x=>x.CreditCardName)
            .NotNull()
            .NotEmpty()
            .WithMessage(PaymentValidationMessages.CreditCardNameRequired);
        
        RuleFor(x=>x.CreditCardExpireDate)
            .NotNull()
            .NotEmpty()
            .WithMessage(PaymentValidationMessages.CreditCardExpireDateRequired);
        
        RuleFor(x=>x.CreditCardCvv2)
            .NotNull()
            .NotEmpty()
            .WithMessage(PaymentValidationMessages.CreditCardCvv2Required);
    }
}