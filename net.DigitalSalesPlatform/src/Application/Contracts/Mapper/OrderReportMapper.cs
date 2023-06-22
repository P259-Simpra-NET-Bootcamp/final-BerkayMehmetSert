using Application.Contracts.Responses.OrderReport;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mapper;

public class OrderReportMapper : Profile
{
    public OrderReportMapper()
    {
        CreateMap<Order, OrderReportResponse>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.CouponAmount, opt => opt.MapFrom(src => src.CouponAmount))
            .ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.CouponCode))
            .ForMember(dest => dest.EarnedPoint, opt => opt.MapFrom(src => src.EarnedPoint))
            .ForMember(dest => dest.SpentPoint, opt => opt.MapFrom(src => src.SpentPoint))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

        CreateMap<OrderDetail, OrderReportDetailResponse>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));
    }
}