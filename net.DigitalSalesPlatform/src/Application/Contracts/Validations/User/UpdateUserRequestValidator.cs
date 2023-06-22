﻿using Application.Contracts.Constants.User;
using Application.Contracts.Requests.User;
using FluentValidation;

namespace Application.Contracts.Validations.User;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .WithMessage(UserValidationMessages.FirstNameRequired)
            .Length(3, 50)
            .WithMessage(UserValidationMessages.FirstNameLength);

        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .WithMessage(UserValidationMessages.LastNameRequired)
            .Length(3, 50)
            .WithMessage(UserValidationMessages.LastNameLength);
    }
}