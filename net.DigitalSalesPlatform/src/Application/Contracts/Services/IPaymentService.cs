using Application.Contracts.Requests.Payment;
using Application.Contracts.Responses.Payment;

namespace Application.Contracts.Services;

public interface IPaymentService
{
    PaymentResponse ProcessPayment(PaymentRequest request);
}