using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class OrderDetailFakeData : BaseFakeData<OrderDetail>
{
    public override List<OrderDetail> CreateFakeData()
    {
        return new List<OrderDetail>
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
            },
            new()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                OrderId = new Guid("11111111-1111-1111-1111-111111111111"),
                Order = new Order { UserId = new Guid("22222222-2222-2222-2222-222222222222") },
                ProductId = new Guid("44444444-4444-4444-4444-444444444444"),
                Product = new Product
                {
                    Id = new Guid("44444444-4444-4444-4444-444444444444"),
                    Name = "Product 2",
                    Price = 200
                }
            },
            new()
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                OrderId = new Guid("11111111-1111-1111-1111-111111111111"),
                Order = new Order { UserId = new Guid("22222222-2222-2222-2222-222222222222") },
                ProductId = new Guid("55555555-5555-5555-5555-555555555555"),
                Product = new Product
                {
                    Id = new Guid("55555555-5555-5555-5555-555555555555"),
                    Name = "Product 3",
                    Price = 300
                }
            }
        };
    }
}