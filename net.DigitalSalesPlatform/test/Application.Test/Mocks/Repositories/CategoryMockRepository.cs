using Application.Contracts.Mapper;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class CategoryMockRepository : BaseMockRepository<ICategoryRepository, Category, CategoryMapper, CategoryFakeData>
{
    public CategoryMockRepository(CategoryFakeData fakeData) : base(fakeData)
    {
    }
}