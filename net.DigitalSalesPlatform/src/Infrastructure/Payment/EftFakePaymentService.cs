using Core.CrossCuttingConcerns.Exceptions.Types;
using Infrastructure.Payment.Constants;
using Infrastructure.Payment.Requests;

namespace Infrastructure.Payment;

public class EftFakePaymentService : IFakePaymentService
{
    public bool Pay(decimal amount, CreditCardRequest request)
    {
        ValidatePaymentDetails(request);

        var paymentResult = ProcessCreditCardPayment(amount, request.CreditCardNumber);

        if (!paymentResult)
            throw new BusinessException(PaymentBusinessMessages.PaymentFailed);

        return true;
    }

    public void Refund(decimal amount, CreditCardRequest request)
    {
        ValidatePaymentDetails(request);
        
        var paymentResult = ProcessCreditCardPayment(amount, request.CreditCardNumber);

        if (!paymentResult)
            throw new BusinessException(PaymentBusinessMessages.RefundFailed);
    }

    private static void ValidatePaymentDetails(CreditCardRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CreditCardNumber))
            throw new BusinessException(PaymentBusinessMessages.CreditCardNumberInvalid);
        if (string.IsNullOrWhiteSpace(request.CreditCardName))
            throw new BusinessException(PaymentBusinessMessages.CreditCardNameInvalid);
        if (string.IsNullOrWhiteSpace(request.CreditCardExpireDate))
            throw new BusinessException(PaymentBusinessMessages.CreditCardExpireDateInvalid);
        if (string.IsNullOrWhiteSpace(request.CreditCardCvv2))
            throw new BusinessException(PaymentBusinessMessages.CreditCardCvv2Invalid);
    }

    private static bool ProcessCreditCardPayment(decimal amount, string maskedCreditCardNumber)
    {
        Console.WriteLine(PaymentBusinessMessages.SuccessfulPaymentEft, amount, maskedCreditCardNumber);
        return true;
    }
}