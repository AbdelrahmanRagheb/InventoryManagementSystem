using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Authentication;

public interface IAuthService
{
    Task<(bool Success, string? Token, string? Error)> Login(string username, string password);
}