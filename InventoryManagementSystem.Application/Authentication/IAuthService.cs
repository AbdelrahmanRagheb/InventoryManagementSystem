using InventoryManagementSystem.Application.Authentication;

namespace InventoryManagementSystem.Application.Authentication;

public interface IAuthService
{
    (bool Success, string? Token, string? Error) Login(string username, string password);
    (bool Success, string? Error) Register(string username, string email, string password, string role);
}