using Application.Contracts.Requests.User;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : BaseAuthController
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService) : base(userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterUserRequest request)
    {
        _userService.RegisterUser(request);
        return Ok();
    }
}