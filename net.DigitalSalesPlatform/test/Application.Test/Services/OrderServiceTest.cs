using Application.Contracts.Constants.Order;
using Application.Contracts.Requests.OrderDetail;
using Application.Contracts.Requests.Payment;
using Application.Contracts.Responses.Cart;
using Application.Contracts.Responses.Payment;
using Application.Contracts.Responses.Product;
using Application.Contracts.Services;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class OrderServiceTest : OrderMockRepository
{
    private readonly OrderService _orderService;
    private readonly Mock<IOrderDetailService> _orderDetailServiceMock = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IPaymentService> _paymentServiceMock = new();

    public OrderServiceTest(OrderFakeData fakeData) : base(fakeData)
    {
        _orderService = new OrderService(
            MockRepository.Object,
            _orderDetailServiceMock.Object,
            _paymentServiceMock.Object,
            _userServiceMock.Object,
            UnitOfWork.Object,
            Mapper
        );
    }

    [Fact]
    public void PayOrderShouldReturnSuccess()
    {
        var paymentRequest = new PaymentRequest();
        var paymentResponse = new PaymentResponse
        {
            UserId = new Guid("22222222-2222-2222-2222-222222222222"),
            CouponCode = "TEST",
            CouponAmount = 10,
            Cart = new CartResponse
            {
                CartItems = new List<CartItem>
                {
                    new()
                    {
                        UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                        Product = new ProductResponse { Id = new Guid("11111111-1111-1111-1111-111111111111") },
                    }
                }
            }
        };
        _paymentServiceMock.Setup(x => x.ProcessPayment(paymentRequest)).Returns(paymentResponse);
        var orderDetailRequest = new CreateOrderDetailRequest
        {
            OrderId = new Guid("11111111-1111-1111-1111-111111111111"),
            ProductId = new Guid("11111111-1111-1111-1111-111111111111")
        };
        _orderDetailServiceMock.Setup(x => x.CreateOrderDetail(orderDetailRequest));
        var order = _orderService.PayOrder(paymentRequest);
        Assert.NotNull(order);
        MockRepository.Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public void GetOrderByNumberShouldReturnSuccess()
    {
        const int orderNumber = 11111111;
        var result = _orderService.GetOrderByNumber(orderNumber);
        Assert.NotNull(result);
        Assert.Equal(orderNumber, result.OrderNumber);
    }

    [Fact]
    public void GetOrderByNumberShouldThrowNotFoundException()
    {
        const int orderNumber = 99999999;
        var exception = Assert.Throws<NotFoundException>(() => _orderService.GetOrderByNumber(orderNumber));
        Assert.Equal(OrderBusinessMessages.OrderNotFoundByOrderNumber, exception.Message);
    }

    [Fact]
    public void GetActiveOrdersShouldReturnSuccess()
    {
        var result = _orderService.GetActiveOrders();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetLastOrdersShouldReturnSuccess()
    {
        var result = _orderService.GetLastOrders();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetAllOrdersShouldReturnSuccess()
    {
        var result = _orderService.GetAllOrders();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetUserActiveOrdersShouldReturnSuccess()
    {
        _userServiceMock.Setup(x => x.GetUserIdFromToken())
            .Returns(new Guid("22222222-2222-2222-2222-222222222222"));
        var result = _orderService.GetUserActiveOrders();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetUserLastOrdersShouldReturnSuccess()
    {
        _userServiceMock.Setup(x => x.GetUserIdFromToken())
            .Returns(new Guid("22222222-2222-2222-2222-222222222222"));
        var result = _orderService.GetUserLastOrders();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetUserAllOrdersShouldReturnSuccess()
    {
        _userServiceMock.Setup(x => x.GetUserIdFromToken())
            .Returns(new Guid("22222222-2222-2222-2222-222222222222"));
        var result = _orderService.GetUserAllOrders();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}