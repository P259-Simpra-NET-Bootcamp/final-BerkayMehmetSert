using Application.Contracts.Responses.OrderDetail;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mapper;

public class OrderDetailMapper : Profile
{
    public OrderDetailMapper()
    {
        CreateMap<OrderDetail, OrderDetailResponse>();
    }
}