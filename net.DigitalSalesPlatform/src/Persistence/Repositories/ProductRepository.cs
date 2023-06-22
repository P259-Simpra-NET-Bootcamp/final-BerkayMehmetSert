using Application.Contracts.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Persistence.Context;

namespace Persistence.Repositories;

public class ProductRepository : BaseRepository<Product, BaseDbContext>, IProductRepository
{
    public ProductRepository(BaseDbContext context, IHttpContextAccessor contextAccessor) : base(context,
        contextAccessor)
    {
    }
}