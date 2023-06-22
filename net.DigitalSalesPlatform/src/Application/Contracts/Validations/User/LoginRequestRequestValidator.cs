using Application.Contracts.Constants.User;
using Application.Contracts.Requests.User;
using FluentValidation;

namespace Application.Contracts.Validations.User;

public class LoginRequestRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestRequestValidator()
    {
        RuleFor(x=>x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage(UserValidationMessages.EmailRequired)
            .MaximumLength(50)
            .WithMessage(UserValidationMessages.EmailLength)
            .EmailAddress()
            .WithMessage(UserValidationMessages.EmailInvalid);
        
        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage(UserValidationMessages.PasswordRequired);
    }
}