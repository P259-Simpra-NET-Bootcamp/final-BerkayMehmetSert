using Application.Contracts.Constants.Product;
using Application.Contracts.Requests.Product;
using FluentValidation;

namespace Application.Contracts.Validations.Product;

public class UpdateProductChangeCategoriesRequestValidator : AbstractValidator<UpdateProductChangeCategoriesRequest>
{
    public UpdateProductChangeCategoriesRequestValidator()
    {
        RuleFor(x=>x.CategoryIds)
            .NotNull()
            .NotEmpty()
            .WithMessage(ProductValidationMessages.CategoryIdsRequired);
    }
}