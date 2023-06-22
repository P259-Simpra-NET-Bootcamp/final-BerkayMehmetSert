using System.ComponentModel.DataAnnotations;
using Application.Contracts.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderReportController : ControllerBase
{
    private readonly IOrderReportService _orderReportService;

    public OrderReportController(IOrderReportService orderReportService)
    {
        _orderReportService = orderReportService;
    }

    [HttpGet("order-number/{orderNumber}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetOrderReportByOrderNumber([FromRoute, Required] int orderNumber)
    {
        var orderReport = _orderReportService.GetOrderReportByOrderNumber(orderNumber);
        return Ok(orderReport);
    }

    [HttpGet("month/{month}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetOrderReportByMonth([FromRoute, Required, Range(1, 12)] int month)
    {
        var orderReport = _orderReportService.GetOrderReportByMonth(month);
        return Ok(orderReport);
    }

    [HttpGet("year/{year}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetOrderReportByYear([FromRoute, Required] int year)
    {
        var orderReport = _orderReportService.GetOrderReportByYear(year);
        return Ok(orderReport);
    }

    [HttpGet("date-range")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetOrderReportByDateRange([FromQuery, Required] DateTime startDate,
        [FromQuery, Required] DateTime endDate)
    {
        var orderReport = _orderReportService.GetOrderReportByDateRange(startDate, endDate);
        return Ok(orderReport);
    }

    [HttpGet("product/{productId}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetOrderReportByProductId([FromRoute, Required] Guid productId)
    {
        var orderReport = _orderReportService.GetOrderReportByProductId(productId);
        return Ok(orderReport);
    }

    [HttpGet("order-status/{orderStatus}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetOrderReportByOrderStatus([FromRoute, Required] OrderStatus orderStatus)
    {
        var orderReport = _orderReportService.GetOrderReportByOrderStatus(orderStatus);
        return Ok(orderReport);
    }
}