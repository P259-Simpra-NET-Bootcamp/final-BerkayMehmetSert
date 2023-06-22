using Application.Contracts.Constants.Category;
using Application.Contracts.Requests.Category;
using FluentValidation;

namespace Application.Contracts.Validations.Category;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(CategoryValidationMessages.NameRequired)
            .Length(3,50)
            .WithMessage(CategoryValidationMessages.NameMaxLength);
            
        RuleFor(x=>x.Description)
            .Length(3,500)
            .WithMessage(CategoryValidationMessages.DescriptionMaxLength);
        
        RuleFor(x=>x.Url)
            .NotNull()
            .NotEmpty()
            .WithMessage(CategoryValidationMessages.UrlRequired)
            .Length(3,200)
            .WithMessage(CategoryValidationMessages.UrlMaxLength);
        
        RuleFor(x=>x.Tags)
            .NotNull()
            .NotEmpty()
            .WithMessage(CategoryValidationMessages.TagsRequired)
            .Length(3,200)
            .WithMessage(CategoryValidationMessages.TagsMaxLength);
        
        RuleFor(x=>x.SortOrder)
            .NotNull()
            .NotEmpty()
            .WithMessage(CategoryValidationMessages.SortOrderRequired);
    }
}