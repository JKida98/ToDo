using ToDo.Attributes;
using ToDo.Database.Models;
using ToDo.Models;
using ToDo.Services;

namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult Authenticate(LoginModel model)
    {
        var response = _userService.Authenticate(model);
        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult Register(RegisterModel model)
    {
        _userService.Register(model);
        return Ok(new { message = "Registration successful" });
    }

    [Authorize(UserRole.Admin)]
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        // only admins can access other user records
        var currentUser = (DbUser)HttpContext.Items["User"]!;
        if (id != currentUser.Id && currentUser.Role != UserRole.Admin)
            return Unauthorized(new { message = "Unauthorized" });

        var user = _userService.GetById(id);
        return Ok(user);
    }
}