using System.Linq.Expressions;
using Application.Contracts.Constants.Order;
using Application.Contracts.Repositories;
using Application.Contracts.Responses.OrderReport;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class OrderReportService : IOrderReportService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderReportService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public OrderReportResponse GetOrderReportByOrderNumber(int orderNumber)
    {
        var order = _orderRepository.Get(
            predicate: x => x.OrderNumber.Equals(orderNumber),
            include: source => source
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
        );
        
        if (order is null)
            throw new NotFoundException(OrderBusinessMessages.OrderNotFoundByOrderNumber);

        return _mapper.Map<OrderReportResponse>(order);
    }

    public List<OrderReportResponse> GetOrderReportByMonth(int month)
    {
        var orders = _orderRepository.GetAll(
            predicate: x => x.OrderDate.Month.Equals(month),
            include: source => source
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
        ).ToList();
        
        if (!orders.Any())
            throw new NotFoundException(OrderBusinessMessages.OrderNotFoundByMonth);

        return _mapper.Map<List<OrderReportResponse>>(orders);
    }

    public List<OrderReportResponse> GetOrderReportByYear(int year)
    {
        var orders = _orderRepository.GetAll(
            predicate: x => x.OrderDate.Year.Equals(year),
            include: source => source
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
        ).ToList();
        
        if (!orders.Any())
            throw new NotFoundException(OrderBusinessMessages.OrderNotFoundByYear);

        return _mapper.Map<List<OrderReportResponse>>(orders);
    }

    public List<OrderReportResponse> GetOrderReportByDateRange(DateTime startDate, DateTime endDate)
    {
        var orders = _orderRepository.GetAll(
            predicate: x => x.OrderDate >= startDate && x.OrderDate <= endDate,
            include: source => source
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
        ).ToList();
        
        if (!orders.Any())
            throw new NotFoundException(OrderBusinessMessages.OrderNotFoundByDateRange);

        return _mapper.Map<List<OrderReportResponse>>(orders);
    }

    public List<OrderReportResponse> GetOrderReportByProductId(Guid productId)
    {
        var orders = _orderRepository.GetAll(
            predicate: x => x.OrderDetails.Any(d => d.ProductId.Equals(productId)),
            include: source => source
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
        ).ToList();
        
        if (!orders.Any())
            throw new NotFoundException(OrderBusinessMessages.OrderNotFoundByProductId);

        return _mapper.Map<List<OrderReportResponse>>(orders);
    }

    public List<OrderReportResponse> GetOrderReportByOrderStatus(OrderStatus orderStatus)
    {
        var orders = _orderRepository.GetAll(
            predicate: x => x.Status.Equals(orderStatus),
            include: source => source
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
        ).ToList();
        
        if (!orders.Any())
            throw new NotFoundException(OrderBusinessMessages.OrderNotFoundByOrderStatus);

        return _mapper.Map<List<OrderReportResponse>>(orders);
    }
}

