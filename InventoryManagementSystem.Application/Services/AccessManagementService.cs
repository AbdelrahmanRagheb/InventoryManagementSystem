using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagementSystem.Application.Authorization;
using InventoryManagementSystem.Application.DTOs.Access;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Services;

public class AccessManagementService : IAccessManagementService
{
    private readonly IUserRepository _userRepo;
    private readonly IRoleRepository _roleRepo;
    private readonly IUserPermissionRepository _permissionRepo;
    private readonly IWarehouseRepository _warehouseRepo;
    private readonly IWarehouseOperatorRepository _operatorRepo;
    private readonly IResourceRepository _resourceRepo;

    public AccessManagementService(
        IUserRepository userRepo,
        IRoleRepository roleRepo,
        IUserPermissionRepository permissionRepo,
        IWarehouseRepository warehouseRepo,
        IWarehouseOperatorRepository operatorRepo,
        IResourceRepository resourceRepo)
    {
        _userRepo = userRepo;
        _roleRepo = roleRepo;
        _permissionRepo = permissionRepo;
        _warehouseRepo = warehouseRepo;
        _operatorRepo = operatorRepo;
        _resourceRepo = resourceRepo;
    }

    public Task<IReadOnlyList<PermissionResponse>> GetPermissionsAsync()
    {
        IReadOnlyList<PermissionResponse> result = PermissionCatalog.Ids
            .Select(kv => new PermissionResponse(
                kv.Value,
                kv.Key,
                PermissionCatalog.Descriptions[kv.Key],
                PermissionCatalog.IsWarehouseScoped(kv.Key)))
            .OrderBy(p => p.Name)
            .ToList();
        return Task.FromResult(result);
    }

    public async Task<IReadOnlyList<RoleResponse>> GetRolesAsync()
    {
        var roles = await _roleRepo.GetAllAsync();
        return roles
            .Select(r => new RoleResponse(r.Id, r.Name, RoleDefaults.DefaultPermissions(r.Name)))
            .ToList();
    }

    public async Task<IReadOnlyList<ResourceResponse>> GetResourcesAsync(string? type)
    {
        var resources = string.IsNullOrWhiteSpace(type)
            ? await _resourceRepo.GetAllAsync()
            : await _resourceRepo.GetByTypeAsync(type);
        return resources.Select(r => new ResourceResponse(r.Id, r.Type, r.Name)).ToList();
    }

    public async Task<IReadOnlyList<AccessUserResponse>> GetUsersAsync()
    {
        var users = await _userRepo.GetAllAsync();
        var warehouseNames = await GetWarehouseNamesAsync();

        var result = new List<AccessUserResponse>();
        foreach (var user in users)
        {
            var grants = await _permissionRepo.GetByUserAsync(user.Id);
            var assignments = await _operatorRepo.GetByOperatorAsync(user.Id);
            result.Add(new AccessUserResponse(
                user.Id,
                user.Username,
                user.Email,
                user.DisplayName,
                user.Role.Name,
                user.IsActive,
                grants.Select(g => new GrantResponse(
                    g.Id,
                    g.Permission.Name,
                    PermissionCatalog.IsWarehouseScoped(g.Permission.Name),
                    g.ResourceId,
                    g.Resource?.Name)).ToList(),
                assignments.Select(a => new AssignedWarehouseResponse(
                    a.WarehouseId,
                    warehouseNames.TryGetValue(a.WarehouseId, out var name) ? name : a.WarehouseId.ToString())).ToList()));
        }
        return result;
    }

    public async Task<(bool Success, string? Error, AccessUserResponse? User)> AssignRoleAsync(Guid userId, Guid roleId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return (false, "User not found", null);

        var role = await _roleRepo.GetByIdAsync(roleId);
        if (role == null) return (false, "Role not found", null);

        var permissions = RoleDefaults.DefaultPermissions(role.Name)
            .Select(name => new UserPermission
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PermissionId = PermissionCatalog.Id(name),
                ResourceId = null
            })
            .ToList();

        await _permissionRepo.ReplaceForUserAsync(userId, permissions);

        user.RoleId = roleId;
        await _userRepo.UpdateAsync(user);

        var updated = await BuildUserResponseAsync(userId);
        return (true, null, updated);
    }

    public async Task<(bool Success, string? Error, GrantResponse? Grant)> GrantAsync(Guid userId, Guid permissionId, Guid? warehouseId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return (false, "User not found", null);

        var permissionName = PermissionCatalog.Ids.FirstOrDefault(kv => kv.Value == permissionId).Key;
        if (permissionName == null) return (false, "Permission not found", null);

        Guid? resourceId = null;
        string? resourceName = null;
        if (warehouseId.HasValue)
        {
            var warehouse = await _warehouseRepo.GetByIdAsync(warehouseId.Value);
            if (warehouse == null) return (false, "Warehouse not found", null);
            if (warehouse.ResourceId == null) return (false, "Warehouse has no linked resource", null);
            resourceId = warehouse.ResourceId;
            resourceName = warehouse.Name;
        }

        var existing = await _permissionRepo.GetAsync(userId, permissionId, resourceId);
        if (existing != null)
            return (true, null, new GrantResponse(
                existing.Id,
                permissionName,
                PermissionCatalog.IsWarehouseScoped(permissionName),
                resourceId,
                resourceName));

        var grant = new UserPermission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PermissionId = permissionId,
            ResourceId = resourceId
        };
        await _permissionRepo.AddAsync(grant);

        return (true, null, new GrantResponse(
            grant.Id,
            permissionName,
            PermissionCatalog.IsWarehouseScoped(permissionName),
            resourceId,
            resourceName));
    }

    public async Task<Guid?> GetGrantOwnerAsync(Guid userPermissionId)
    {
        var existing = await _permissionRepo.GetByIdAsync(userPermissionId);
        return existing?.UserId;
    }

    public async Task<bool> RevokeAsync(Guid userPermissionId)
    {
        var existing = await _permissionRepo.GetByIdAsync(userPermissionId);
        if (existing == null) return false;
        await _permissionRepo.RemoveAsync(userPermissionId);
        return true;
    }

    private async Task<AccessUserResponse> BuildUserResponseAsync(Guid userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        var grants = await _permissionRepo.GetByUserAsync(userId);
        var assignments = await _operatorRepo.GetByOperatorAsync(userId);
        var warehouseNames = await GetWarehouseNamesAsync();

        return new AccessUserResponse(
            user!.Id,
            user.Username,
            user.Email,
            user.DisplayName,
            user.Role.Name,
            user.IsActive,
            grants.Select(g => new GrantResponse(
                g.Id,
                g.Permission.Name,
                PermissionCatalog.IsWarehouseScoped(g.Permission.Name),
                g.ResourceId,
                g.Resource?.Name)).ToList(),
            assignments.Select(a => new AssignedWarehouseResponse(
                a.WarehouseId,
                warehouseNames.TryGetValue(a.WarehouseId, out var name) ? name : a.WarehouseId.ToString())).ToList());
    }

    private async Task<IReadOnlyDictionary<Guid, string>> GetWarehouseNamesAsync()
    {
        var warehouses = await _warehouseRepo.GetAllAsync();
        return warehouses.ToDictionary(w => w.Id, w => w.Name);
    }
}