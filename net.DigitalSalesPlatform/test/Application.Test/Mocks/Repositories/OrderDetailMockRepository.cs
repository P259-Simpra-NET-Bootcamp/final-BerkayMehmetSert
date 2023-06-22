using Application.Contracts.Mapper;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class OrderDetailMockRepository : BaseMockRepository<IOrderDetailRepository, OrderDetail,OrderDetailMapper, OrderDetailFakeData>
{
    public OrderDetailMockRepository(OrderDetailFakeData fakeData) : base(fakeData)
    {
    }
}