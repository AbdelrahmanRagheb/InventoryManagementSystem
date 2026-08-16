namespace InventoryManagementSystem.Application.DTOs.Users;

public record CreateUserRequest(string Username, string Email, string Password, string Role, string DisplayName);

public record UpdateUserRequest(Guid Id, string? Username = null, string? Email = null, string? DisplayName = null, bool? IsActive = null);

public record UserResponse(Guid Id, string Username, string Email, string DisplayName, string Role, bool IsActive, DateTime CreatedAt);