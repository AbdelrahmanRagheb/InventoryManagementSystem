using System;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagementSystem.Application.Authorization;
using InventoryManagementSystem.Application.Repositories;

namespace InventoryManagementSystem.Application.Services;

public class AccessService : IAccessService
{
    private readonly IUserRepository _userRepo;
    private readonly IUserPermissionRepository _permissionRepo;
    private readonly IWarehouseOperatorRepository _warehouseOperatorRepo;
    private readonly IWarehouseRepository _warehouseRepo;

    public AccessService(
        IUserRepository userRepo,
        IUserPermissionRepository permissionRepo,
        IWarehouseOperatorRepository warehouseOperatorRepo,
        IWarehouseRepository warehouseRepo)
    {
        _userRepo = userRepo;
        _permissionRepo = permissionRepo;
        _warehouseOperatorRepo = warehouseOperatorRepo;
        _warehouseRepo = warehouseRepo;
    }

    public async Task<string?> GetRoleNameAsync(Guid userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        return user == null || !user.IsActive ? null : user.Role.Name;
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string permission)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null || !user.IsActive)
            return false;
        if (user.Role.Name == RoleDefaults.Admin)
            return true;

        var permissions = await _permissionRepo.GetByUserAsync(userId);
        return permissions.Any(up => up.Permission.Name == permission);
    }

    public async Task<bool> CanAsync(Guid userId, string permission, Guid? warehouseId = null)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null || !user.IsActive)
            return false;
        if (user.Role.Name == RoleDefaults.Admin)
            return true;

        var permissions = await _permissionRepo.GetByUserAsync(userId);
        var hasGlobal = permissions.Any(up => up.Permission.Name == permission && up.ResourceId == null);

        if (!PermissionCatalog.IsWarehouseScoped(permission))
            return hasGlobal || permissions.Any(up => up.Permission.Name == permission);

        if (!warehouseId.HasValue)
            return hasGlobal;

        var warehouse = await _warehouseRepo.GetByIdAsync(warehouseId.Value);
        if (warehouse == null)
            return false;

        var hasScopedMatch = permissions.Any(up =>
            up.Permission.Name == permission &&
            up.ResourceId != null &&
            up.ResourceId == warehouse.ResourceId);
        if (hasScopedMatch)
            return true;

        if (!hasGlobal)
            return false;

        if (user.Role.Name == RoleDefaults.WarehouseOperator)
        {
            var assignments = await _warehouseOperatorRepo.GetByOperatorAsync(userId);
            return assignments.Any(a => a.WarehouseId == warehouseId.Value);
        }

        return true;
    }
}