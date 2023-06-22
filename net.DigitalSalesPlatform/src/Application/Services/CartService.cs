using Application.Contracts.Constants.Cart;
using Application.Contracts.Requests.Cart;
using Application.Contracts.Responses.Cart;
using Application.Contracts.Responses.Product;
using Application.Contracts.Services;
using AutoMapper;
using Core.Application.Caching;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;

namespace Application.Services;

public class CartService : ICartService
{
    private readonly IMapper _mapper;
    private readonly Func<CacheType, ICacheService> _cacheService;
    private readonly IProductService _productService;
    private readonly IUserService _userService;

    private const CacheType Cache = CacheType.Redis;

    public CartService(
        IMapper mapper,
        Func<CacheType, ICacheService> cacheService,
        IProductService productService,
        IUserService userService)
    {
        _mapper = mapper;
        _cacheService = cacheService;
        _productService = productService;
        _userService = userService;
    }

    public void AddToCart(AddToCartRequest request)
    {
        var userId = GetUserIdFromToken();
        var cacheKey = GetCacheKey(userId);
        var cartItems = GetCartItemsFromCache(cacheKey);

        var existingCartItem = cartItems.Find(item => item.Product.Id == request.ProductId);
        var newQuantity = (existingCartItem?.Quantity ?? 0) + request.Quantity;
        ValidateCartItemStock(request.ProductId, newQuantity);

        if (existingCartItem is not null)
        {
            existingCartItem.Quantity = newQuantity;
        }
        else
        {
            var product = GetProductById(request.ProductId);
            var productResponse = _mapper.Map<ProductResponse>(product);
            var newCartItem = CreateCartItem(userId, productResponse, request.Quantity);
            cartItems.Add(newCartItem);
        }

        SetCartItemsToCache(cacheKey, cartItems);
    }

    public void RemoveFromCart(Guid productId)
    {
        var cacheKey = GetCacheKey(GetUserIdFromToken());
        var cartItems = GetCartItemsFromCache(cacheKey);
        var product = GetProductById(productId);

        var existingCartItem = cartItems.Find(item => item.Product.Id == product.Id);

        if (existingCartItem is not null)
        {
            if (existingCartItem.Quantity > 1)
            {
                existingCartItem.Quantity -= 1;
            }
            else
            {
                cartItems.Remove(existingCartItem);
            }
        }

        SetCartItemsToCache(cacheKey, cartItems);
    }

    public void ClearCart()
    {
        var cacheKey = GetCacheKey(GetUserIdFromToken());
        _cacheService(Cache).Remove(cacheKey);
    }

    public CartResponse GetCartItems()
    {
        var cacheKey = GetCacheKey(GetUserIdFromToken());
        var cartItems = GetCartItemsFromCache(cacheKey);

        var response = new CartResponse{ CartItems = cartItems };

        CalculateCartTotalPrice(response, cartItems);

        return response;
    }

    private Guid GetUserIdFromToken() => _userService.GetUserIdFromToken();

    private string GetCacheKey(Guid userId) => $"cart:{userId}";

    private List<CartItem> GetCartItemsFromCache(string cacheKey)
    {
        return _cacheService(Cache).TryGet(cacheKey, out List<CartItem> cartItems) ? cartItems : new List<CartItem>();
    }

    private void SetCartItemsToCache(string cacheKey, List<CartItem> cartItems)
    {
        _cacheService(Cache).Set(cacheKey, cartItems);
    }

    private void ValidateCartItemStock(Guid productId, int quantity)
    {
        var product = GetProductById(productId);
        if (quantity > product.Stock)
            throw new BusinessException(CartBusinessMessages.InvalidStock);
    }

    private Product GetProductById(Guid productId) => _productService.GetActiveProductEntity(productId);

    private CartItem CreateCartItem(Guid userId, ProductResponse productResponse, int quantity)
    {
        return new CartItem
        {
            UserId = userId,
            Product = productResponse,
            Quantity = quantity
        };
    }

    private void CalculateCartTotalPrice(CartResponse response, List<CartItem> cartItems)
    {
        foreach (var cartItem in cartItems)
        {
            var product = GetProductById(cartItem.Product.Id);
            response.TotalPrice += product.Price * cartItem.Quantity;
        }
    }
}