using Application.Contracts.Requests.Coupon;
using Application.Contracts.Responses.Coupon;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface ICouponService
{
    void CreateCoupon(CreateCouponRequest request);
    void UpdateCouponIsUsed(string code);
    void UpdateCouponChangeUsed(Guid id);
    void UpdateCouponStatus(Guid id, bool isActive);
    CouponResponse GetCouponByCode(string code);
    List<CouponResponse> GetAllCoupons();
    List<CouponResponse> GetAllActiveCoupons();
    Coupon GetCouponEntityByCode(string code);
}