using Application.Contracts.Constants.Coupon;
using Application.Contracts.Requests.Coupon;
using Core.Utilities.Date;
using FluentValidation;

namespace Application.Contracts.Validations.Coupon;

public class CreateCouponRequestValidator : AbstractValidator<CreateCouponRequest>
{
    public CreateCouponRequestValidator()
    {
        RuleFor(x => x.Amount)
            .NotNull()
            .NotEmpty()
            .WithMessage(CouponValidationMessages.AmountRequired)
            .GreaterThan(0)
            .WithMessage(CouponValidationMessages.AmountGreaterThanZero);

        RuleFor(x => x.ExpirationDate)
            .NotNull()
            .NotEmpty()
            .WithMessage(CouponValidationMessages.ExpirationDateRequired)
            .GreaterThan(DateHelper.GetCurrentDate());
    }
}