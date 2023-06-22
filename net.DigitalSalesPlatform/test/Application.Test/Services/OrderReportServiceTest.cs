using Application.Contracts.Constants.Order;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Enums;
using Xunit;

namespace Application.Test.Services;

public class OrderReportServiceTest : OrderReportMockRepository
{
    private readonly OrderReportService _orderReportService;

    public OrderReportServiceTest(OrderReportFakeData fakeData) : base(fakeData)
    {
        _orderReportService = new OrderReportService(
            MockRepository.Object,
            Mapper
        );
    }

    [Fact]
    public void GetOrderReportByOrderNumberValidRequestShouldReturnSuccess()
    {
        const int orderNumber = 111111;
        var result = _orderReportService.GetOrderReportByOrderNumber(orderNumber);
        Assert.NotNull(result);
        Assert.Equal(orderNumber, result.OrderNumber);
    }

    [Fact]
    public void GetOrderReportByOrderNumberValidRequestShouldThrowNotFoundException()
    {
        const int orderNumber = 222222;
        var exception = Assert.Throws<NotFoundException>(() =>
            _orderReportService.GetOrderReportByOrderNumber(orderNumber)
        );
        Assert.Equal(OrderBusinessMessages.OrderNotFoundByOrderNumber, exception.Message);
    }
    
    [Fact]
    public void GetOrderReportByMonthShouldReturnSuccess()
    {
        const int month = 1;
        var result = _orderReportService.GetOrderReportByMonth(month);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }
    
    [Fact]
    public void GetOrderReportByMonthShouldThrowNotFoundException()
    {
        const int month = 2;
        var exception = Assert.Throws<NotFoundException>(() =>
            _orderReportService.GetOrderReportByMonth(month)
        );
        Assert.Equal(OrderBusinessMessages.OrderNotFoundByMonth, exception.Message);
    }
    
    [Fact]
    public void GetOrderReportByYearShouldReturnSuccess()
    {
        const int year = 2023;
        var result = _orderReportService.GetOrderReportByYear(year);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }
    
    [Fact]
    public void GetOrderReportByYearShouldThrowNotFoundException()
    {
        const int year = 2022;
        var exception = Assert.Throws<NotFoundException>(() =>
            _orderReportService.GetOrderReportByYear(year)
        );
        Assert.Equal(OrderBusinessMessages.OrderNotFoundByYear, exception.Message);
    }

    [Fact]
    public void GetOrderReportByDateRangeShouldReturnSuccess()
    {
        var startDate = new DateTime(2023, 1, 1 );
        var endDate = new DateTime(2023, 1, 31);
        var result = _orderReportService.GetOrderReportByDateRange(startDate, endDate);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    
    [Fact]
    public void GetOrderReportByDateRangeShouldThrowNotFoundException()
    {
        var startDate = new DateTime(2022, 1, 1 );
        var endDate = new DateTime(2022, 1, 31);
        var exception = Assert.Throws<NotFoundException>(() =>
            _orderReportService.GetOrderReportByDateRange(startDate, endDate)
        );
        Assert.Equal(OrderBusinessMessages.OrderNotFoundByDateRange, exception.Message);
    }
    
    [Fact]
    public void GetOrderReportByProductIdShouldReturnSuccess()
    {
        var productId = new Guid("33333333-3333-3333-3333-333333333333");
        var result = _orderReportService.GetOrderReportByProductId(productId);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    
    [Fact]
    public void GetOrderReportByProductIdShouldThrowNotFoundException()
    {
        var productId = new Guid("44444444-4444-4444-4444-444444444444");
        var exception = Assert.Throws<NotFoundException>(() =>
            _orderReportService.GetOrderReportByProductId(productId)
        );
        Assert.Equal(OrderBusinessMessages.OrderNotFoundByProductId, exception.Message);
    }

    [Fact]
    public void GetOrderReportByOrderStatusShouldReturnSuccess()
    {
        const OrderStatus orderStatus = OrderStatus.Pending;
        var result = _orderReportService.GetOrderReportByOrderStatus(orderStatus);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    
    [Fact]
    public void GetOrderReportByOrderStatusShouldThrowNotFoundException()
    {
        const OrderStatus orderStatus = OrderStatus.Shipped;
        var exception = Assert.Throws<NotFoundException>(() =>
            _orderReportService.GetOrderReportByOrderStatus(orderStatus)
        );
        Assert.Equal(OrderBusinessMessages.OrderNotFoundByOrderStatus, exception.Message);
    }
}