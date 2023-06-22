using Core.Test.FakeData;
using Domain.Entities;
using Domain.Enums;

namespace Application.Test.Mocks.FakeData;

public class OrderReportFakeData : BaseFakeData<Order>
{
    public override List<Order> CreateFakeData()
    {
        return new List<Order>()
        {
            new()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                OrderNumber = 111111,
                OrderDate = new DateTime(2023, 1, 1),
                OrderDetails = new List<OrderDetail>
                {
                    new()
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111111"),
                        OrderId = new Guid("11111111-1111-1111-1111-111111111111"),
                        Order = new Order { UserId = new Guid("22222222-2222-2222-2222-222222222222") },
                        ProductId = new Guid("33333333-3333-3333-3333-333333333333"),
                        Product = new Product
                        {
                            Id = new Guid("33333333-3333-3333-3333-333333333333"),
                            Name = "Product 1",
                            Price = 100
                        }
                    }
                },
                UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                User = new User
                {
                    Id = new Guid("22222222-2222-2222-2222-222222222222"),
                    FirstName = "User",
                    LastName = "1"
                },
                Status = OrderStatus.Pending,
            }
        };
    }
}