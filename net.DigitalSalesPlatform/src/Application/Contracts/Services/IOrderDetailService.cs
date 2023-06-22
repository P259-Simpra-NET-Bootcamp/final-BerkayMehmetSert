using Application.Contracts.Requests.OrderDetail;
using Application.Contracts.Responses.OrderDetail;

namespace Application.Contracts.Services;

public interface IOrderDetailService
{
    void CreateOrderDetail(CreateOrderDetailRequest request);
    OrderDetailResponse GetOrderDetailById(Guid id);
    List<OrderDetailResponse> GetOrderDetailsByOrderId(Guid orderId);
    List<OrderDetailResponse> GetOrderDetailsByProductId(Guid productId);
    List<OrderDetailResponse> GetOrderDetailsByUserId();
}