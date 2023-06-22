namespace Application.Contracts.Responses.Cart;

public class CartResponse
{
    public List<CartItem> CartItems { get; set; }
    public decimal TotalPrice { get; set; }
}