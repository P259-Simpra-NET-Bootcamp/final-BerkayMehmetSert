using Application.Contracts.Requests.Coupon;
using Application.Contracts.Responses.Coupon;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mapper;

public class CouponMapper : Profile
{
    public CouponMapper()
    {
        CreateMap<CreateCouponRequest, Coupon>();
        CreateMap<Coupon, CouponResponse>();
    }
}