using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryManagementSystem.Application.DTOs.Access;

namespace InventoryManagementSystem.Application.Services;

public interface IAccessManagementService
{
    Task<IReadOnlyList<PermissionResponse>> GetPermissionsAsync();
    Task<IReadOnlyList<RoleResponse>> GetRolesAsync();
    Task<IReadOnlyList<ResourceResponse>> GetResourcesAsync(string? type);
    Task<IReadOnlyList<AccessUserResponse>> GetUsersAsync();
    Task<(bool Success, string? Error, AccessUserResponse? User)> AssignRoleAsync(Guid userId, Guid roleId);
    Task<(bool Success, string? Error, GrantResponse? Grant)> GrantAsync(Guid userId, Guid permissionId, Guid? warehouseId);
    Task<Guid?> GetGrantOwnerAsync(Guid userPermissionId);
    Task<bool> RevokeAsync(Guid userPermissionId);
}