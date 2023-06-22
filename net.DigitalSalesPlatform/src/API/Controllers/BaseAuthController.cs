using Application.Contracts.Requests.User;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public abstract class BaseAuthController : ControllerBase
{
    private readonly IUserService _userService;
    public BaseAuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _userService.Login(request);
        return Ok(user);
    }
}