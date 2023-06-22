using Application.Contracts.Constants.Product;
using Application.Contracts.Requests.Product;
using FluentValidation;

namespace Application.Contracts.Validations.Product;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
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
    }
}