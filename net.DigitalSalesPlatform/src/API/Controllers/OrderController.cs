using System.ComponentModel.DataAnnotations;
using Application.Contracts.Requests.Payment;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin, User")]
    public IActionResult PayOrder([FromBody] PaymentRequest request)
    {
        var order = _orderService.PayOrder(request);

        return Ok(order);
    }
    
    [HttpGet("{orderNumber}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetOrderByNumber([FromRoute, Required] int orderNumber)
    {
        var order = _orderService.GetOrderByNumber(orderNumber);
        return Ok(order);
    }

    [HttpGet("active")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetActiveOrders()
    {
        var orders = _orderService.GetActiveOrders();
        return Ok(orders);
    }

    [HttpGet("last")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetLastOrders()
    {
        var orders = _orderService.GetLastOrders();
        return Ok(orders);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllOrders()
    {
        var orders = _orderService.GetAllOrders();
        return Ok(orders);
    }
    
    [HttpGet("user/active")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetUserActiveOrders()
    {
        var orders = _orderService.GetUserActiveOrders();
        return Ok(orders);
    }

    [HttpGet("user/last")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetUserLastOrders()
    {
        var orders = _orderService.GetUserLastOrders();
        return Ok(orders);
    }

    [HttpGet("user/all")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetUserAllOrders()
    {
        var orders = _orderService.GetUserAllOrders();
        return Ok(orders);
    }
}