using Application.Contracts.Constants.Cart;
using Application.Contracts.Requests.Cart;
using FluentValidation;

namespace Application.Contracts.Validations.Cart;

public class AddToCartRequestValidator : AbstractValidator<AddToCartRequest>
{
    public AddToCartRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .NotEmpty()
            .WithMessage(CartValidationMessages.ProductIdRequired);

        RuleFor(x => x.Quantity)
            .NotNull()
            .NotEmpty()
            .WithMessage(CartValidationMessages.QuantityRequired)
            .GreaterThan(0)
            .WithMessage(CartValidationMessages.QuantityGreaterThanZero);
    }
}