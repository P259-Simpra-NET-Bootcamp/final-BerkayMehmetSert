using Application.Contracts.Requests.User;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut]
    [Authorize(Roles = "Admin, User")]
    public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
    {
        _userService.UpdateUser(request);
        return Ok();
    }

    [HttpPut("password")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult UpdateUserPassword([FromBody] ChangePasswordRequest request)
    {
        _userService.ChangePassword(request);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetMyUserInfo()
    {
        var user = _userService.GetUserById();
        return Ok(user);
    }

    [HttpGet("point")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetMyPoint()
    {
        var user = _userService.GetUserPointById();
        return Ok(user);
    }
}