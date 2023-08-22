using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TinyCRM.Application.Models.User;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Infrastructure.Serilog;

namespace TinyCRM.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync(SignInDto signInDto)
    {
        var result = await _authService.SignInAsync(signInDto);
        LoggerService.LogInformation(
            $"[{DateTime.Now}]Successfully Signed In User: {JsonSerializer.Serialize(result)}");
        return Ok(new { Token = result });
    }
}