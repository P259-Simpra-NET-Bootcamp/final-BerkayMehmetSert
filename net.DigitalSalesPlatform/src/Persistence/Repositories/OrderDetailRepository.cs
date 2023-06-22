using Application.Contracts.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Persistence.Context;

namespace Persistence.Repositories;

public class OrderDetailRepository : BaseRepository<OrderDetail, BaseDbContext>, IOrderDetailRepository
{
    public OrderDetailRepository(BaseDbContext context, IHttpContextAccessor contextAccessor) : base(context,
        contextAccessor)
    {
    }
}