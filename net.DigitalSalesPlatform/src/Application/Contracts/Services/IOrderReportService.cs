using Application.Contracts.Responses.OrderReport;
using Domain.Enums;

namespace Application.Contracts.Services;

public interface IOrderReportService
{
    OrderReportResponse GetOrderReportByOrderNumber(int orderNumber);
    List<OrderReportResponse> GetOrderReportByMonth(int month);
    List<OrderReportResponse> GetOrderReportByYear(int year);
    List<OrderReportResponse> GetOrderReportByDateRange(DateTime startDate, DateTime endDate);
    List<OrderReportResponse> GetOrderReportByProductId(Guid productId);
    List<OrderReportResponse> GetOrderReportByOrderStatus(OrderStatus orderStatus);
}