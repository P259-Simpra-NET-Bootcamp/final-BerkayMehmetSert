using Application.Contracts.Constants.Product;
using Application.Contracts.Requests.Product;
using Application.Contracts.Services;
using Application.Contracts.Validations.Product;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class ProductServiceTest : ProductMockRepository
{
    private readonly ProductService _productService;
    private readonly Mock<ICategoryService> _categoryServiceMock = new();
    private readonly Mock<PointCalculationService> _pointCalculationServiceMock = new();
    private readonly UpdateProductRequestValidator _updateProductRequestValidator;
    private readonly UpdateProductPriceRequestValidator _updateProductPriceRequestValidator;
    
    public ProductServiceTest(
        ProductFakeData fakeData,
        UpdateProductRequestValidator updateProductRequestValidator,
        UpdateProductPriceRequestValidator updateProductPriceRequestValidator) : base(fakeData)
    {
        _updateProductRequestValidator = updateProductRequestValidator;
        _updateProductPriceRequestValidator = updateProductPriceRequestValidator;
        _productService = new ProductService(
            MockRepository.Object,
            UnitOfWork.Object,
            _categoryServiceMock.Object,
            _pointCalculationServiceMock.Object,
            Mapper
        );
    }

    [Fact]
    public void UpdateProductValidRequestShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new UpdateProductRequest
        {
            Name = "Updated Product", 
            Description = "Updated Description",
            Features = "Updated Features"
        };
        _productService.UpdateProduct(id, request);

        MockRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public void UpdateProductValidRequestShouldThrowNotFoundException()
    {
        var id = Guid.Empty;
        var request = new UpdateProductRequest
        {
            Name = "Updated Product", 
            Description = "Updated Description",
            Features = "Updated Features"
        };
        var exception = Assert.Throws<NotFoundException>(() => _productService.UpdateProduct(id, request));
        Assert.Equal(ProductBusinessMessages.ProductNotFoundById, exception.Message);
    }

    [Fact]
    public void UpdateProductValidRequestShouldThrowNotActiveException()
    {
        var id = new Guid("33333333-3333-3333-3333-333333333333");
        var request = new UpdateProductRequest
        {
            Name = "Updated Product",
            Description = "Updated Description",
            Features = "Updated Features"
        };
        var exception = Assert.Throws<BusinessException>(() => _productService.UpdateProduct(id, request));
        Assert.Equal(ProductBusinessMessages.ProductIsNotActive, exception.Message);
    }
    [Fact]
    public void UpdateProductInValidRequestShouldThrowValidationException()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new UpdateProductRequest
        {
            Name = "",
            Description = "Updated Description",
            Features = "Updated Features"
        };
        var result = _updateProductRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("Name"))
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        _productService.UpdateProduct(id, request);
        Assert.NotNull(result);
        Assert.Equal(ProductValidationMessages.NameRequired, result);
    }

    [Fact]
    public void UpdateProductStatusShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        const bool isActive = true;
        _productService.UpdateProductStatus(id, isActive);
        MockRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public void UpdateProductStatusShouldThrowNotFoundException()
    {
        var id = Guid.Empty;
        const bool isActive = true;
        var exception = Assert.Throws<NotFoundException>(() => _productService.UpdateProductStatus(id, isActive));
        Assert.Equal(ProductBusinessMessages.ProductNotFoundById, exception.Message);
    }

    [Fact]
    public void UpdateProductPriceValidRequestShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new UpdateProductPriceRequest { Price = 100 };
        _productService.UpdateProductPrice(id, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public void UpdateProductPriceValidRequestShouldThrowNotFoundException()
    {
        var id = Guid.Empty;
        var request = new UpdateProductPriceRequest { Price = 100 };
        var exception = Assert.Throws<NotFoundException>(() => _productService.UpdateProductPrice(id, request));
        Assert.Equal(ProductBusinessMessages.ProductNotFoundById, exception.Message);
    }

    [Fact]
    public void UpdateProductPriceValidRequestShouldThrowNotActiveException()
    {
        var id = new Guid("33333333-3333-3333-3333-333333333333");
        var request = new UpdateProductPriceRequest { Price = 100 };
        var exception = Assert.Throws<BusinessException>(() => _productService.UpdateProductPrice(id, request));
        Assert.Equal(ProductBusinessMessages.ProductIsNotActive, exception.Message);
    }

    [Fact]
    public void UpdateProductPriceInValidRequestShouldThrowValidationException()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new UpdateProductPriceRequest { Price = -100 };
        var result = _updateProductPriceRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("Price"))
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        _productService.UpdateProductPrice(id, request);
        Assert.NotNull(result);
        Assert.Equal(ProductValidationMessages.PriceGreaterThanZero, result);
    }

    [Fact]
    public void UpdateProductStockIncrementShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        const int stock = 10;
        _productService.UpdateProductStockIncrement(id, stock);
        MockRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
    }
    
    [Fact]
    public void UpdateProductStockIncrementShouldThrowNotFoundException()
    {
        var id = Guid.Empty;
        const int stock = 10;
        var exception = Assert.Throws<NotFoundException>(() => _productService.UpdateProductStockIncrement(id, stock));
        Assert.Equal(ProductBusinessMessages.ProductNotFoundById, exception.Message);
    }
    
    [Fact]
    public void UpdateProductStockIncrementShouldThrowNotActiveException()
    {
        var id = new Guid("33333333-3333-3333-3333-333333333333");
        const int stock = 10;
        var exception = Assert.Throws<BusinessException>(() => _productService.UpdateProductStockIncrement(id, stock));
        Assert.Equal(ProductBusinessMessages.ProductIsNotActive, exception.Message);
    }
    
    [Fact]
    public void UpdateProductStockDecrementShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        const int stock = 10;
        _productService.UpdateProductStockDecrement(id, stock);
        MockRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
    }
    
    [Fact]
    public void UpdateProductStockDecrementShouldThrowNotFoundException()
    {
        var id = Guid.Empty;
        const int stock = 10;
        var exception = Assert.Throws<NotFoundException>(() => _productService.UpdateProductStockDecrement(id, stock));
        Assert.Equal(ProductBusinessMessages.ProductNotFoundById, exception.Message);
    }
    
    [Fact]
    public void UpdateProductStockDecrementShouldThrowNotActiveException()
    {
        var id = new Guid("33333333-3333-3333-3333-333333333333");
        const int stock = 10;
        var exception = Assert.Throws<BusinessException>(() => _productService.UpdateProductStockDecrement(id, stock));
        Assert.Equal(ProductBusinessMessages.ProductIsNotActive, exception.Message);
    }
    
    [Fact]
    public void DeleteProductShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        _productService.DeleteProduct(id);
        MockRepository.Verify(x => x.Delete(It.IsAny<Product>()), Times.Once);
    }
    
    [Fact]
    public void DeleteProductShouldThrowNotFoundException()
    {
        var id = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _productService.DeleteProduct(id));
        Assert.Equal(ProductBusinessMessages.ProductNotFoundById, exception.Message);
    }

    [Fact]
    public void GetProductByIdShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _productService.GetProductById(id);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }
    
    [Fact]
    public void GetProductByIdShouldThrowNotFoundException()
    {
        var id = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _productService.GetProductById(id));
        Assert.Equal(ProductBusinessMessages.ProductNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetProductByIdShouldThrowNotActiveException()
    {
        var id = new Guid("33333333-3333-3333-3333-333333333333");
        var exception = Assert.Throws<BusinessException>(() => _productService.GetProductById(id));
        Assert.Equal(ProductBusinessMessages.ProductIsNotActive, exception.Message);
    }
    
    [Fact]
    public void GetActiveProductEntityShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _productService.GetActiveProductEntity(id);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }
    
    [Fact]
    public void GetActiveProductEntityShouldThrowNotFoundException()
    {
        var id = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _productService.GetActiveProductEntity(id));
        Assert.Equal(ProductBusinessMessages.ProductNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetActiveProductEntityShouldThrowNotActiveException()
    {
        var id = new Guid("33333333-3333-3333-3333-333333333333");
        var exception = Assert.Throws<BusinessException>(() => _productService.GetActiveProductEntity(id));
        Assert.Equal(ProductBusinessMessages.ProductIsNotActive, exception.Message);
    }

    [Fact]
    public void GetAllProductShouldReturnSuccess()
    {
        var result = _productService.GetAllProducts();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}