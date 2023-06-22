using Core.Application.Request;

namespace Application.Contracts.Requests.Cart;

public class AddToCartRequest : BaseRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}