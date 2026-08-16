namespace InventoryManagementSystem.Application.DTOs.Auth;

public record LoginRequest(string Username, string Password);

public record LoginResponse(string Token);

public record RegisterRequest(string Username, string Email, string Password, string Role);