using Application.Contracts.Responses.Cart;
using Core.Application.Request;

namespace Application.Contracts.Requests.Order;

public class CreateOrderRequest : BaseRequest
{
    public Guid UserId { get; set; }
    public string? CouponCode { get; set; }
    public decimal CouponAmount { get; set; }
    public decimal EarnedPoint { get; set; }
    public decimal SpentPoint { get; set; }
    public CartResponse Cart { get; set; }
}