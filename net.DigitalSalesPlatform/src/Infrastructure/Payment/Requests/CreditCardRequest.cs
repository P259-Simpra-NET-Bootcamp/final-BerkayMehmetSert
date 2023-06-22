namespace Infrastructure.Payment.Requests;

public class CreditCardRequest
{
    public string CreditCardNumber { get; set; }
    public string CreditCardName { get; set; }
    public string CreditCardExpireDate { get; set; }
    public string CreditCardCvv2 { get; set; }
}