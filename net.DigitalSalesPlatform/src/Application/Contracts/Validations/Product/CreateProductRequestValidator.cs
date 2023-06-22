using Application.Contracts.Constants.Product;
using Application.Contracts.Requests.Product;
using FluentValidation;

namespace Application.Contracts.Validations.Product;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x=>x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(ProductValidationMessages.NameRequired)
            .Length(3, 250)
            .WithMessage(ProductValidationMessages.NameLength);
        
        RuleFor(x=>x.Description)
            .NotNull()
            .NotEmpty()
            .WithMessage(ProductValidationMessages.DescriptionRequired)
            .Length(3, 250)
            .WithMessage(ProductValidationMessages.DescriptionLength);
        
        RuleFor(x=>x.Features)
            .NotNull()
            .NotEmpty()
            .WithMessage(ProductValidationMessages.FeaturesRequired)
            .Length(3, 250)
            .WithMessage(ProductValidationMessages.FeaturesLength);
        
        RuleFor(x=>x.Price)
            .NotNull()
            .NotEmpty()
            .WithMessage(ProductValidationMessages.PriceRequired)
            .GreaterThan(0)
            .WithMessage(ProductValidationMessages.PriceGreaterThanZero);
        
        RuleFor(x=>x.Stock)
            .NotNull()
            .NotEmpty()
            .WithMessage(ProductValidationMessages.StockRequired)
            .GreaterThan(0)
            .WithMessage(ProductValidationMessages.StockGreaterThanZero);
        
        RuleFor(x=>x.CategoryIds)
            .NotNull()
            .NotEmpty()
            .WithMessage(ProductValidationMessages.CategoryIdsRequired);
    }
}