using Domain.Enums;

namespace Application.Contracts.Responses.OrderReport;

public class OrderReportResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal CouponAmount { get; set; }
    public string CouponCode { get; set; }
    public decimal EarnedPoint { get; set; }
    public decimal SpentPoint { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderReportDetailResponse>? OrderDetails { get; set; }
}