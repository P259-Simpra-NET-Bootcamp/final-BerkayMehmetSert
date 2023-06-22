using Application.Contracts.Mapper;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class OrderMockRepository : BaseMockRepository<IOrderRepository, Order, OrderMapper, OrderFakeData>
{
    public OrderMockRepository(OrderFakeData fakeData) : base(fakeData)
    {
    }
}