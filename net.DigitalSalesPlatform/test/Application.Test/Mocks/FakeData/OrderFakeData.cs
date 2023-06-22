using Core.Test.FakeData;
using Domain.Entities;
using Domain.Enums;

namespace Application.Test.Mocks.FakeData;

public class OrderFakeData : BaseFakeData<Order>
{
    public override List<Order> CreateFakeData()
    {
        return new List<Order>
        {
            new ()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                User = new User { Id = new Guid("22222222-2222-2222-2222-222222222222"), },
                OrderNumber = 11111111,
                TotalAmount = 600,
                CouponAmount = 0,
                CouponCode = null,
                OrderDate = new DateTime(2023, 1, 1),
                Status = OrderStatus.Pending
            },
            new ()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                User = new User { Id = new Guid("22222222-2222-2222-2222-222222222222"), },
                OrderNumber = 22222222,
                TotalAmount = 600,
                CouponAmount = 0,
                CouponCode = null,
                OrderDate = new DateTime(2023, 6, 19),
                Status = OrderStatus.Pending
            },
        };
    }
}