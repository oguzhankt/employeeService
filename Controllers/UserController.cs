using EmployeeService.Models.Requests;
using EmployeeService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Route("create")]
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUser createUserDto)
    {
        var result = await _userService.CreateUserAsync(createUserDto);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        var result = await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}