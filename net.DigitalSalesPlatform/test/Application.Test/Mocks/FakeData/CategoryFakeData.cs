using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class CategoryFakeData : BaseFakeData<Category>
{
    public override List<Category> CreateFakeData()
    {
        return new List<Category>()
        {
            new()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Category 1",
                Description = "Category 1 Description",
                Url = "category-1",
                Tags = "#Category 1 Tags",
                SortOrder = 1,
                IsActive = true
            },
            new ()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Category 2",
                Description = "Category 2 Description",
                Url = "category-2",
                Tags = "#Category 2 Tags",
                SortOrder = 2,
                IsActive = true,
                ProductCategories = new List<ProductCategoryMap>
                {
                    new()
                    {
                        ProductId = new Guid("11111111-1111-1111-1111-111111111111"),
                        CategoryId = new Guid("22222222-2222-2222-2222-222222222222")
                    }
                }
            },
            new ()
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                Name = "Category 3",
                Description = "Category 3 Description",
                Url = "category-3",
                Tags = "#Category 3 Tags",
                SortOrder = 3,
                IsActive = false
            },
        };
    }
}