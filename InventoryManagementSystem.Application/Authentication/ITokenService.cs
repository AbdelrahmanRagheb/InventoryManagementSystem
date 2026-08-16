namespace InventoryManagementSystem.Application.Authentication;

public interface ITokenService
{
    string GenerateToken(string userId, string username, string role);
}