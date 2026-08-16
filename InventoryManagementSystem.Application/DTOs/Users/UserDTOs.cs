using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Application.DTOs.Users;

public record CreateUserRequest(
    [param: Required, StringLength(50)] string Username,
    [param: Required, EmailAddress, StringLength(150)] string Email,
    [param: Required] string Password,
    [param: Required, StringLength(50)] string Role,
    [param: Required, StringLength(150)] string DisplayName);

public record UpdateUserRequest(
    Guid Id,
    [param: StringLength(50)] string? Username = null,
    [param: EmailAddress, StringLength(150)] string? Email = null,
    [param: StringLength(150)] string? DisplayName = null,
    bool? IsActive = null);

public record UserResponse(Guid Id, string Username, string Email, string DisplayName, string Role, bool IsActive, DateTime CreatedAt);