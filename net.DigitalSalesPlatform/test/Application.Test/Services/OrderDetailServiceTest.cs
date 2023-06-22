using Application.Contracts.Constants.OrderDetail;
using Application.Contracts.Requests.OrderDetail;
using Application.Contracts.Services;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class OrderDetailServiceTest : OrderDetailMockRepository
{
    private readonly OrderDetailService _orderDetailService;
    private readonly Mock<IUserService> _userServiceMock = new();

    public OrderDetailServiceTest(OrderDetailFakeData fakeData) : base(fakeData)
    {
        _userServiceMock.Setup(x => x.GetUserIdFromToken())
            .Returns(new Guid("22222222-2222-2222-2222-222222222222"));
        _orderDetailService = new OrderDetailService(
            MockRepository.Object,
            _userServiceMock.Object,
            UnitOfWork.Object,
            Mapper
        );
    }

    [Fact]
    public void CreateOrderDetailShouldReturnSuccess()
    {
        var request = new CreateOrderDetailRequest { OrderId = new Guid("11111111-1111-1111-1111-111111111111") };
        _orderDetailService.CreateOrderDetail(request);
        MockRepository.Verify(x => x.Add(It.IsAny<OrderDetail>()), Times.Once);
    }

    [Fact]
    public void GetOrderDetailByIdShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _orderDetailService.GetOrderDetailById(id);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void GetOrderDetailByIdShouldThrowNotFoundException()
    {
        var id = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _orderDetailService.GetOrderDetailById(id));
        Assert.Equal(OrderDetailBusinessMessages.OrderDetailNotFoundById, exception.Message);
    }

    [Fact]
    public void GetOrderDetailsByOrderIdShouldReturnSuccess()
    {
        var orderId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _orderDetailService.GetOrderDetailsByOrderId(orderId);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetOrderDetailsByOrderIdShouldThrowNotFoundException()
    {
        var orderId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _orderDetailService.GetOrderDetailsByOrderId(orderId));
        Assert.Equal(OrderDetailBusinessMessages.OrderDetailNotFoundByOrderId, exception.Message);
    }

    [Fact]
    public void GetOrderDetailsByProductIdShouldReturnSuccess()
    {
        var productId = new Guid("33333333-3333-3333-3333-333333333333");
        var result = _orderDetailService.GetOrderDetailsByProductId(productId);
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public void GetOrderDetailsByProductIdShouldThrowNotFoundException()
    {
        var productId = Guid.Empty;
        var exception =
            Assert.Throws<NotFoundException>(() => _orderDetailService.GetOrderDetailsByProductId(productId));
        Assert.Equal(OrderDetailBusinessMessages.OrderDetailNotFoundByProductId, exception.Message);
    }

    [Fact]
    public void GetOrderDetailsByUserIdShouldReturnSuccess()
    {
        var result = _orderDetailService.GetOrderDetailsByUserId();
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetOrderDetailsByUserIdShouldThrowNotFoundException()
    {
        var userId = Guid.Empty;
        _userServiceMock.Setup(x => x.GetUserIdFromToken()).Returns(userId);
        var exception = Assert.Throws<NotFoundException>(() => _orderDetailService.GetOrderDetailsByUserId());
        Assert.Equal(OrderDetailBusinessMessages.OrderDetailNotFoundByUserId, exception.Message);
    }
}