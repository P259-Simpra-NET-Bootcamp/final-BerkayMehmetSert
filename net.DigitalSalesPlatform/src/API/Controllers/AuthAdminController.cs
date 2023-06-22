using Application.Contracts.Requests.User;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthAdminController : BaseAuthController
{
    private readonly IUserService _userService;

    public AuthAdminController(IUserService userService) : base(userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    public IActionResult Register([FromBody] RegisterAdminRequest request)
    {
        _userService.RegisterAdmin(request);
        return Ok();
    }
}