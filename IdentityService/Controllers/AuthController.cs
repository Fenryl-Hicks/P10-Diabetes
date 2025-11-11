using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Services.Interfaces;

namespace IdentityService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    public record LoginRequest(string Username, string Password);
    public record RegisterRequest(string Username, string Email, string Password);

    [HttpPost("token")]
    public async Task<IActionResult> GetToken([FromBody] LoginRequest request)
    {
        var token = await _authService.GenerateTokenAsync(request.Username, request.Password);

        if (token is null)
            return Unauthorized();

        return Ok(new
        {
            access_token = token,
            token_type = "Bearer",
            expires_in = 3600
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterUserAsync(request.Username, request.Email, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { Message = "Utilisateur enregistré avec succès" });
    }
}
