using InventoryManagementSystem.Application.Authentication;
using InventoryManagementSystem.Application.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (success, token, error) = await _authService.Login(request.Username, request.Password);
        if (!success) return Unauthorized(new { error });
        return Ok(new LoginResponse(token!));
    }
}