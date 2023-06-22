using System.ComponentModel.DataAnnotations;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpDelete("user/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteUser([FromRoute, Required] Guid id)
    {
        _userService.DeleteUser(id);
        return Ok();
    }

    [HttpGet("user/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetUserById([FromRoute, Required] Guid id)
    {
        var user = _userService.GetUserById(id);
        return Ok(user);
    }

    [HttpGet("user/{id}/point")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetUserPointById([FromRoute, Required] Guid id)
    {
        var user = _userService.GetUserPointById(id);
        return Ok(user);
    }

    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }
}