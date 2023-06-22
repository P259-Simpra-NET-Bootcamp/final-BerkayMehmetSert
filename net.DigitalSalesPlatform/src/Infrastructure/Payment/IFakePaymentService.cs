using Infrastructure.Payment.Requests;

namespace Infrastructure.Payment;

public interface IFakePaymentService
{
    bool Pay(decimal amount, CreditCardRequest request);
    void Refund(decimal amount, CreditCardRequest request);
}