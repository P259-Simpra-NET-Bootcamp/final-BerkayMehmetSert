using Application.Contracts.Responses.Product;

namespace Application.Contracts.Responses.Cart;

public class CartItem
{
    public Guid UserId { get; set; }
    public ProductResponse Product { get; set; }
    public int Quantity { get; set; }
}