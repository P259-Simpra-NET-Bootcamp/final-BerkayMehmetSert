using Core.Application.Response;

namespace Application.Contracts.Responses.Coupon;

public class CouponResponse : BaseResponse
{
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsUsed { get; set; }
    public bool IsActive { get; set; }
}