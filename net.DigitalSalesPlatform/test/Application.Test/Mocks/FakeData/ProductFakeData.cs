using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class ProductFakeData : BaseFakeData<Product>
{
    public override List<Product> CreateFakeData()
    {
        return new List<Product>
        {
            new ()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Product 1",
                Stock = 100,
                Price = 100,
                IsActive = true,
                ProductCategories = new List<ProductCategoryMap>
                {
                    new ()
                    {
                        CategoryId = new Guid("11111111-1111-1111-1111-111111111111"),
                        Category = new Category
                        {
                            Id = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "Category 1",
                            IsActive = true
                        },
                        ProductId = new Guid("11111111-1111-1111-1111-111111111111")
                    }
                }
            },
            new ()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Product 2",
                Stock = 0,
                Price = 100,
                IsActive = true,
                ProductCategories = new List<ProductCategoryMap>
                {
                    new ()
                    {
                        CategoryId = new Guid("22222222-2222-2222-2222-222222222222"),
                        Category = new Category
                        {
                            Id = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = "Category 2",
                            IsActive = true
                        },
                        ProductId = new Guid("22222222-2222-2222-2222-222222222222")
                    }
                }
            },
            new ()
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                Name = "Product 3",
                Stock = 100,
                Price = 100,
                IsActive = false
            }
        };
    }
}