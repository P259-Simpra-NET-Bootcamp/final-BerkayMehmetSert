using Application.Contracts.Responses.Cart;

namespace Application.Contracts.Responses.Payment;

public class PaymentResponse
{
    public Guid UserId { get; set; }
    public string? CouponCode { get; set; }
    public decimal CouponAmount { get; set; }
    public decimal EarnedPoint { get; set; }
    public decimal SpentPoint { get; set; }
    public CartResponse Cart { get; set; }
}