namespace Application.Contracts.Constants.Coupon;

public static class CouponBusinessMessages
{
    public const string ExpirationDateIsGreaterThanToday = "The coupon expiration date cannot be before today.";
    public const string CouponNotFoundByCode = "The coupon with the provided code was not found.";
    public const string CouponNotFoundById = "The coupon with the provided id was not found.";
    public const string CouponIsNotActive = "The provided coupon code is not active.";
    public const string CouponExpired = "The coupon has expired.";
    public const string CouponIsUsed = "The coupon code has already been used.";
}