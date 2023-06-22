using Application.Contracts.Constants.OrderDetail;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.OrderDetail;
using Application.Contracts.Responses.OrderDetail;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services;

public class OrderDetailService : IOrderDetailService
{
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderDetailService(
        IOrderDetailRepository orderDetailRepository,
        IUserService userService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _orderDetailRepository = orderDetailRepository;
        _userService = userService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public void CreateOrderDetail(CreateOrderDetailRequest request)
    {
        var orderDetail = new OrderDetail
        {
            OrderId = request.OrderId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            Price = request.Price,
            TotalAmount = request.TotalAmount
        };

        _orderDetailRepository.Add(orderDetail);
        _unitOfWork.SaveChanges();
    }

    public OrderDetailResponse GetOrderDetailById(Guid id)
    {
        var orderDetail = _orderDetailRepository.Get(predicate: x => x.Id.Equals(id));

        if (orderDetail is null)
            throw new NotFoundException(OrderDetailBusinessMessages.OrderDetailNotFoundById);

        return _mapper.Map<OrderDetailResponse>(orderDetail);
    }

    public List<OrderDetailResponse> GetOrderDetailsByOrderId(Guid orderId)
    {
        var userId = _userService.GetUserIdFromToken();
        var orderDetails = _orderDetailRepository.GetAll(
            predicate: x => x.OrderId.Equals(orderId) && x.Order.UserId.Equals(userId)
        );

        if (!orderDetails.Any())
            throw new NotFoundException(OrderDetailBusinessMessages.OrderDetailNotFoundByOrderId);

        return _mapper.Map<List<OrderDetailResponse>>(orderDetails);
    }

    public List<OrderDetailResponse> GetOrderDetailsByProductId(Guid productId)
    {
        var userId = _userService.GetUserIdFromToken();
        var orderDetails =
            _orderDetailRepository.GetAll(
                predicate: x => x.ProductId.Equals(productId) && x.Order.UserId.Equals(userId)
            );

        if (!orderDetails.Any())
            throw new NotFoundException(OrderDetailBusinessMessages.OrderDetailNotFoundByProductId);

        return _mapper.Map<List<OrderDetailResponse>>(orderDetails);
    }

    public List<OrderDetailResponse> GetOrderDetailsByUserId()
    {
        var userId = _userService.GetUserIdFromToken();
        var orderDetails = _orderDetailRepository.GetAll(predicate: x => x.Order.UserId.Equals(userId));

        if (!orderDetails.Any())
            throw new NotFoundException(OrderDetailBusinessMessages.OrderDetailNotFoundByUserId);

        return _mapper.Map<List<OrderDetailResponse>>(orderDetails);
    }
}