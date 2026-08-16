using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Application.DTOs.Auth;

public record LoginRequest(
    [param: Required] string Username,
    [param: Required] string Password);

public record LoginResponse(string Token);

public record RegisterRequest(
    [param: Required, StringLength(50)] string Username,
    [param: Required, EmailAddress, StringLength(150)] string Email,
    [param: Required] string Password,
    [param: Required, StringLength(50)] string Role);