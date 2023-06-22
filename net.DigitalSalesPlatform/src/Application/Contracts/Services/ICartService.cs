using Application.Contracts.Requests.Cart;
using Application.Contracts.Responses.Cart;

namespace Application.Contracts.Services;

public interface ICartService
{
    void AddToCart(AddToCartRequest request);
    void RemoveFromCart(Guid productId);
    void ClearCart();
    CartResponse GetCartItems();
}