using Application.Contracts.Requests.Product;
using Application.Contracts.Responses.Product;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface IProductService
{
    void CreateProduct(CreateProductRequest request);
    void UpdateProduct(Guid id, UpdateProductRequest request);
    void UpdateProductStatus(Guid id, bool isActive);
    void UpdateProductPrice(Guid id, UpdateProductPriceRequest request);
    void UpdateProductCategories(Guid id, UpdateProductChangeCategoriesRequest request);
    void UpdateProductStockIncrement(Guid id, int stock);
    void UpdateProductStockDecrement(Guid id, int stock);
    void DeleteProduct(Guid id);
    ProductResponse GetProductById(Guid id);
    List<ProductResponse> GetProductsByCategoryId(Guid categoryId);
    List<ProductResponse> GetAllProducts();
    Product GetActiveProductEntity(Guid id);
}