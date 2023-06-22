using Application.Contracts.Requests.Payment;
using FluentValidation;

namespace Application.Contracts.Validations.Payment;

public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
{
    public PaymentRequestValidator()
    {
        RuleFor(x => x.CreditCard)
            .SetValidator(new CreditCardRequestValidator());
    }
}