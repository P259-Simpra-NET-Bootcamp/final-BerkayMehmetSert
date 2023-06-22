using Application.Contracts.Mapper;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class OrderReportMockRepository : BaseMockRepository<IOrderRepository, Order, OrderReportMapper, OrderReportFakeData>
{
    public OrderReportMockRepository(OrderReportFakeData fakeData) : base(fakeData)
    {
    }
}