using System.ComponentModel.DataAnnotations;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailService _orderDetailService;

    public OrderDetailController(IOrderDetailService orderDetailService)
    {
        _orderDetailService = orderDetailService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetOrderDetailById([FromRoute, Required] Guid id)
    {
        var orderDetail = _orderDetailService.GetOrderDetailById(id);
        return Ok(orderDetail);
    }

    [HttpGet("order/{orderId}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetOrderDetailsByOrderId([FromRoute, Required] Guid orderId)
    {
        var orderDetails = _orderDetailService.GetOrderDetailsByOrderId(orderId);
        return Ok(orderDetails);
    }

    [HttpGet("product/{productId}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetOrderDetailsByProductId([FromRoute, Required] Guid productId)
    {
        var orderDetails = _orderDetailService.GetOrderDetailsByProductId(productId);
        return Ok(orderDetails);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetOrderDetailsByUserId()
    {
        var orderDetails = _orderDetailService.GetOrderDetailsByUserId();
        return Ok(orderDetails);
    }
}