using Application.Contracts.Constants.Product;
using Application.Contracts.Requests.Product;
using FluentValidation;

namespace Application.Contracts.Validations.Product;

public class UpdateProductPriceRequestValidator : AbstractValidator<UpdateProductPriceRequest>
{
    public UpdateProductPriceRequestValidator()
    {
        RuleFor(x=>x.Price)
            .NotNull()
            .NotEmpty()
            .WithMessage(ProductValidationMessages.PriceRequired)
            .GreaterThan(0)
            .WithMessage(ProductValidationMessages.PriceGreaterThanZero);
    }
}