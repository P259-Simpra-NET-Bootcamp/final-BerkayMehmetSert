using Application.Contracts.Mapper;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class CouponMockRepository : BaseMockRepository<ICouponRepository, Coupon, CouponMapper, CouponFakeData>
{
    public CouponMockRepository(CouponFakeData fakeData) : base(fakeData)
    {
    }
}