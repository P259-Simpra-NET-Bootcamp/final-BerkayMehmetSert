using Core.Application.Response;

namespace Application.Contracts.Responses.OrderDetail;

public class OrderDetailResponse : BaseResponse
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
}