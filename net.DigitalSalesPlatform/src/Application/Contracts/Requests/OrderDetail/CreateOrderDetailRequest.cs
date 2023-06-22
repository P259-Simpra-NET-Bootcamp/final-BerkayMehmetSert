using Core.Application.Request;

namespace Application.Contracts.Requests.OrderDetail;

public class CreateOrderDetailRequest : BaseRequest
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
}