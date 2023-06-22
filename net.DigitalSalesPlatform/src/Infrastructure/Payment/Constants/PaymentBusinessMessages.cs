namespace Infrastructure.Payment.Constants;

public static class PaymentBusinessMessages
{
    public const string PaymentCompleted = "Payment completed successfully. Thank you for your purchase.";
    public const string PaymentFailed = "Payment failed. Please check your payment details and try again.";
    public const string RefundFailed = "Refund failed. Please check your payment details and try again.";

    public const string CreditCardNumberInvalid =
        "CreditCardNumber is required. Please provide a valid credit card number.";

    public const string CreditCardNameInvalid = "CreditCardName is required. Please provide a valid credit card name.";

    public const string CreditCardExpireDateInvalid =
        "CreditCardExpireDate is required. Please provide a valid credit card expire date.";

    public const string CreditCardCvv2Invalid = "CreditCardCvv2 is required. Please provide a valid credit card CVV2.";

    public const string SuccessfulPaymentCreditCard =
        "Credit card payment processed successfully. Transaction amount: {0}. Masked credit card number: {1}.";
    public const string SuccessfulPaymentEft =
        "EFT payment processed successfully. Transaction amount: {0}. Masked bank account number: {1}.";
}