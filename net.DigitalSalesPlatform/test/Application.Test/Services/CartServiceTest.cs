using Application.Contracts.Constants.Cart;
using Application.Contracts.Requests.Cart;
using Application.Contracts.Responses.Cart;
using Application.Contracts.Responses.Product;
using Application.Contracts.Services;
using Application.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Test.Helper;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class CartServiceTest
{
    private readonly CartService _cartService;
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IProductService> _productServiceMock = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    
    public CartServiceTest()
    {
        _userServiceMock.Setup(x => x.GetUserIdFromToken()).Returns(new Guid("11111111-1111-1111-1111-111111111111"));
        var cacheService = CacheMockHelper.GetCacheService();
        _cartService = new CartService(_mapperMock.Object, cacheService, _productServiceMock.Object,
            _userServiceMock.Object);
    }

    [Fact]
    public void AddToCartShouldReturnSuccess()
    {
        var request = new AddToCartRequest
        {
            ProductId = new Guid("22222222-2222-2222-2222-222222222222"),
            Quantity = 1
        };
        var cartItems = new List<CartItem>();
        var existingCartItem = new CartItem
        {
            Product = new ProductResponse() { Id = request.ProductId },
            Quantity = 2
        };
        cartItems.Add(existingCartItem);
        var product = new Product { Id = request.ProductId, Stock = 10, Price = 100 };
        _productServiceMock.Setup(x => x.GetActiveProductEntity(request.ProductId)).Returns(product);
        _cartService.AddToCart(request);
        Assert.Single(cartItems);
    }
    
    [Fact]
    public void AddToCartShouldThrowStockException()
    {
        var request = new AddToCartRequest
        {
            ProductId = new Guid("22222222-2222-2222-2222-222222222222"),
            Quantity = 1
        };
        var product = new Product { Id = request.ProductId, Stock = 0, Price = 100 };
        _productServiceMock.Setup(x => x.GetActiveProductEntity(request.ProductId)).Returns(product);
        var exception = Assert.Throws<BusinessException>(() => _cartService.AddToCart(request));
        Assert.Equal(CartBusinessMessages.InvalidStock, exception.Message);
    }

    [Fact]
    public void ClearCartShouldReturnSuccess()
    {
        _cartService.ClearCart();
        _productServiceMock.Verify(x => x.GetActiveProductEntity(It.IsAny<Guid>()), Times.Never);
    }
}

