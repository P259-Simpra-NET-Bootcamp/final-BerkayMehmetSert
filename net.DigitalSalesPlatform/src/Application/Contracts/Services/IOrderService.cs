using Application.Contracts.Requests.Payment;
using Application.Contracts.Responses.Order;

namespace Application.Contracts.Services;

public interface IOrderService
{
    OrderResponse PayOrder(PaymentRequest request);
    OrderResponse GetOrderByNumber(int orderNumber);
    List<OrderResponse> GetActiveOrders();
    List<OrderResponse> GetLastOrders();
    List<OrderResponse> GetAllOrders();
    List<OrderResponse> GetUserActiveOrders();
    List<OrderResponse> GetUserLastOrders();
    List<OrderResponse> GetUserAllOrders();
}