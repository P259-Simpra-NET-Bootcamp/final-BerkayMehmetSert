using Core.Application.Request;
using Infrastructure.Payment.Requests;

namespace Application.Contracts.Requests.Payment;

public class PaymentRequest : BaseRequest
{
    public string CouponCode { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public CreditCardRequest CreditCard { get; set; }
}