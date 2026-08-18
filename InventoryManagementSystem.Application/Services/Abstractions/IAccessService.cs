using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IAccessService
{
    Task<bool> HasPermissionAsync(Guid userId, string permission);
    Task<bool> CanAsync(Guid userId, string permission, Guid? warehouseId = null);
    Task<string?> GetRoleNameAsync(Guid userId);
}