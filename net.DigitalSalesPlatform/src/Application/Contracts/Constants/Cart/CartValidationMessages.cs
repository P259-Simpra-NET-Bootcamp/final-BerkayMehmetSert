namespace Application.Contracts.Constants.Cart;

public static class CartValidationMessages
{
    public const string ProductIdRequired = "ProductId is required";
    public const string QuantityRequired = "Quantity is required";
    public const string QuantityGreaterThanZero = "Quantity must be greater than 0";
}