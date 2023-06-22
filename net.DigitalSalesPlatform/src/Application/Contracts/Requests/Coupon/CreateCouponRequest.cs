using Core.Application.Request;

namespace Application.Contracts.Requests.Coupon;

public class CreateCouponRequest : BaseRequest
{
    public decimal Amount { get; set; }
    public DateTime ExpirationDate { get; set; }
}