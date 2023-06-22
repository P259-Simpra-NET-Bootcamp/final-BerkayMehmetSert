using Application.Contracts.Responses.Order;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mapper;

public class OrderMapper : Profile
{
    public OrderMapper()
    {
        CreateMap<Order, OrderResponse>();
    }
}