using System.ComponentModel.DataAnnotations;
using Application.Contracts.Requests.Coupon;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CouponController : ControllerBase
{
    private readonly ICouponService _couponService;

    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateCoupon([FromBody] CreateCouponRequest request)
    {
        _couponService.CreateCoupon(request);
        return Ok();
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateCouponStatus([FromRoute] Guid id, [FromQuery, Required] bool isActive)
    {
        _couponService.UpdateCouponStatus(id, isActive);
        return Ok();
    }
    
    [HttpGet("{code}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetCouponByCode([FromRoute, Required] string code)
    {
        var coupon = _couponService.GetCouponByCode(code);
        return Ok(coupon);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllCoupons()
    {
        var coupons = _couponService.GetAllCoupons();
        return Ok(coupons);
    }

    [HttpGet("active")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllActiveCoupons()
    {
        var coupons = _couponService.GetAllActiveCoupons();
        return Ok(coupons);
    }
}