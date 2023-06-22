using Application.Contracts.Responses.OrderDetail;
using Core.Application.Response;
using Domain.Enums;

namespace Application.Contracts.Responses.Order;

public class OrderResponse : BaseResponse
{
    public int OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal CouponAmount { get; set; }
    public string? CouponCode { get; set; }
    public decimal EarnedPoint { get; set; }
    public decimal SpentPoint { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderDetailResponse>? OrderDetails { get; set; }
}