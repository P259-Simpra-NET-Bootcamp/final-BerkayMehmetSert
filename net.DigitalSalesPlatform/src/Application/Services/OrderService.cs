using Application.Contracts.Constants.Order;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Order;
using Application.Contracts.Requests.OrderDetail;
using Application.Contracts.Requests.Payment;
using Application.Contracts.Responses.Cart;
using Application.Contracts.Responses.Order;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence.Repositories;
using Core.Utilities.Code;
using Core.Utilities.Date;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailService _orderDetailService;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPaymentService _paymentService;

    public OrderService(
        IOrderRepository orderRepository,
        IOrderDetailService orderDetailService,
        IPaymentService paymentService,
        IUserService userService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _orderDetailService = orderDetailService;
        _paymentService = paymentService;
        _userService = userService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public OrderResponse PayOrder(PaymentRequest request)
    {
        var paymentService = _paymentService.ProcessPayment(request);
        var orderRequest = new CreateOrderRequest
        {
            UserId = paymentService.UserId,
            CouponCode = paymentService.CouponCode,
            CouponAmount = paymentService.CouponAmount,
            EarnedPoint = paymentService.EarnedPoint,
            SpentPoint = paymentService.SpentPoint,
            Cart = paymentService.Cart
        };

        var order = CreateNewOrder(orderRequest);

        _orderRepository.Add(order);
        _unitOfWork.SaveChanges();

        CreateOrderDetails(order, orderRequest.Cart.CartItems);

        return _mapper.Map<OrderResponse>(order);
    }

    public OrderResponse GetOrderByNumber(int orderNumber)
    {
        var order = _orderRepository.Get(
            predicate: x => x.OrderNumber.Equals(orderNumber),
            include: source => source
                .Include(x => x.OrderDetails)
        );

        if (order is null)
            throw new NotFoundException(OrderBusinessMessages.OrderNotFoundByOrderNumber);

        return _mapper.Map<OrderResponse>(order);
    }

    public List<OrderResponse> GetActiveOrders()
    {
        var orders = _orderRepository.GetAll(
            predicate: x => x.Status.Equals(OrderStatus.Pending),
            include: source => source
                .Include(x => x.OrderDetails)
        );

        return _mapper.Map<List<OrderResponse>>(orders);
    }

    public List<OrderResponse> GetLastOrders()
    {
        var orders = _orderRepository.GetAll(predicate: x => x.OrderDate <= DateHelper.GetPreviousDate(),
            include: source => source
                .Include(x => x.OrderDetails)
        );

        return _mapper.Map<List<OrderResponse>>(orders);
    }

    public List<OrderResponse> GetAllOrders()
    {
        var orders = _orderRepository.GetAll(
            include: source => source
                .Include(x => x.OrderDetails)
        );

        return _mapper.Map<List<OrderResponse>>(orders);
    }
    
    public List<OrderResponse> GetUserActiveOrders()
    {
        var userId = _userService.GetUserIdFromToken();
        var orders = _orderRepository.GetAll(
            predicate: x => x.Status.Equals(OrderStatus.Pending) && x.UserId.Equals(userId),
            include: source => source
                .Include(x => x.OrderDetails)
        );

        return _mapper.Map<List<OrderResponse>>(orders);
    }

    public List<OrderResponse> GetUserLastOrders()
    {
        var userId = _userService.GetUserIdFromToken();
        var orders = _orderRepository.GetAll(
            predicate: x => x.OrderDate <= DateHelper.GetPreviousDate() && x.UserId.Equals(userId),
            include: source => source
                .Include(x => x.OrderDetails)
        );

        return _mapper.Map<List<OrderResponse>>(orders);
    }
    
    public List<OrderResponse> GetUserAllOrders()
    {
        var userId = _userService.GetUserIdFromToken();
        var orders = _orderRepository.GetAll(
            predicate: x => x.UserId.Equals(userId),
            include: source => source
                .Include(x => x.OrderDetails)
        );

        return _mapper.Map<List<OrderResponse>>(orders);
    }

    private static Order CreateNewOrder(CreateOrderRequest request)
    {
        var order = new Order
        {
            OrderNumber = OrderNumberGenerator.GenerateOrderNumber(),
            UserId = request.UserId,
            TotalAmount = request.Cart.TotalPrice,
            EarnedPoint = request.EarnedPoint,
            SpentPoint = request.SpentPoint
        };

        if (!string.IsNullOrEmpty(request.CouponCode) && !string.IsNullOrWhiteSpace(request.CouponCode))
        {
            order.CouponCode = request.CouponCode;
            order.CouponAmount = request.CouponAmount;
        }

        order.OrderDate = DateHelper.GetCurrentDate();

        return order;
    }

    private void CreateOrderDetails(Order order, List<CartItem> cartItems)
    {
        foreach (var orderDetail in cartItems.Select(cartItem => CreateOrderDetail(cartItem, order.Id)))
        {
            _orderDetailService.CreateOrderDetail(orderDetail);
        }
    }

    private static CreateOrderDetailRequest CreateOrderDetail(CartItem cartItem, Guid orderId)
    {
        return new CreateOrderDetailRequest
        {
            OrderId = orderId,
            ProductId = cartItem.Product.Id,
            Quantity = cartItem.Quantity,
            Price = cartItem.Product.Price,
            TotalAmount = cartItem.Product.Price * cartItem.Quantity
        };
    }
}