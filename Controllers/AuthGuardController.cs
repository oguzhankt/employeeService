using EmployeeService.Models;
using EmployeeService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthGuardController : Controller
{
    private readonly IAuthGuardService _authGuardService;

    public AuthGuardController(IAuthGuardService authGuardService)
    {
        _authGuardService = authGuardService;
    }
    
    [Route("createToken")]
    [HttpPost]
    public async Task<IActionResult> CreateToken(Login login)
    {
        var result = await _authGuardService.CreateTokenAsync(login);

        if (result == null)
        {
            return BadRequest("Invalid login");
        }
        
        return Ok(result);
    }

    [Route("createTokenByClient")]
    [HttpPost]
    public IActionResult CreateTokenByClient(ClientLogin clientLogin)
    {
        var result = _authGuardService.CreateTokenByClient(clientLogin);

        if (result == null)
        {
            return BadRequest("Invalid login");
        }
        
        return Ok(result);
    }

    [Route("revokeToken")]
    [HttpPost]
    public async Task<IActionResult> RevokeRefreshToken(RefreshToken refreshToken)
    {
        var result = _authGuardService.RevokeRefreshToken(refreshToken.Token);

        if (result)
        {
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }
    
    [Route("createByRefreshToken")]
    [HttpPost]
    public async Task<IActionResult> CreateTokenByRefreshToken(RefreshToken refreshToken)

    {
        var result = await _authGuardService.CreateTokenByRefreshToken(refreshToken.Token);
        
        if (result == null)
        {
            return BadRequest("Invalid refresh token");
        }
        
        return Ok(result);
    }
}