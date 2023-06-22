using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class CouponFakeData : BaseFakeData<Coupon>
{
    public override List<Coupon> CreateFakeData()
    {
        return new List<Coupon>
        {
            new()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Code = "11111111",
                Amount = 10,
                ExpirationDate = DateTime.Now.AddDays(10),
                IsActive = true,
                IsUsed = false
            },
            new()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Code = "22222222",
                Amount = 20,
                ExpirationDate = DateTime.Now.AddDays(-10),
                IsActive = true,
                IsUsed = true
            },
            new()
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                Code = "33333333",
                Amount = 30,
                ExpirationDate = DateTime.Now.AddDays(30),
                IsActive = false,
                IsUsed = true
            },
            new()
            {
                Id = new Guid("44444444-4444-4444-4444-444444444444"),
                Code = "44444444",
                Amount = 40,
                ExpirationDate = DateTime.Now.AddDays(40),
                IsActive = true,
                IsUsed = true
            }
        };
    }
}