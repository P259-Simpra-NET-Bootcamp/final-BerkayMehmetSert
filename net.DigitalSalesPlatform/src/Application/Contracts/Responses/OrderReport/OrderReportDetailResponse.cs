using Core.Application.Response;

namespace Application.Contracts.Responses.OrderReport;

public class OrderReportDetailResponse : BaseResponse
{
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
}