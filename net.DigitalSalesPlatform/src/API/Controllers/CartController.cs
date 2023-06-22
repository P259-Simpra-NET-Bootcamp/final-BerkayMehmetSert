using System.ComponentModel.DataAnnotations;
using Application.Contracts.Requests.Cart;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin, User")]
    public IActionResult AddToCart([FromBody] AddToCartRequest request)
    {
        _cartService.AddToCart(request);
        return Ok();
    }

    [HttpPut("remove/{productId}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult RemoveFromCart([FromRoute, Required] Guid productId)
    {
        _cartService.RemoveFromCart(productId);
        return Ok();
    }

    [HttpDelete("clear")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult ClearCart()
    {
        _cartService.ClearCart();
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetCartItems()
    {
        var cartItems = _cartService.GetCartItems();
        return Ok(cartItems);
    }
}