using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Application.DTOs.Auth;

public record LoginRequest(
    [param: Required] string Username,
    [param: Required] string Password);

public record LoginResponse(string Token);