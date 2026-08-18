using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IAccessService
{
    Task<bool> HasPermissionAsync(Guid userId, string permission);
    Task<bool> CanAsync(Guid userId, string permission, Guid? warehouseId = null);
    Task<string?> GetRoleNameAsync(Guid userId);
    Task<bool> IsRestrictedToAssignedWarehousesAsync(Guid userId);
    Task<IReadOnlyList<Guid>> GetAssignedWarehouseIdsAsync(Guid userId);
}