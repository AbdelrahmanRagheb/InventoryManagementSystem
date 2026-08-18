using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Application.DTOs.Access;

public record PermissionResponse(Guid Id, string Name, string Description, bool WarehouseScoped);

public record RoleResponse(Guid Id, string Name, IReadOnlyList<string> DefaultPermissions);

public record ResourceResponse(Guid Id, string Type, string Name);

public record GrantResponse(
    Guid Id,
    string Permission,
    bool WarehouseScoped,
    Guid? ResourceId,
    string? ResourceName);

public record AssignedWarehouseResponse(Guid WarehouseId, string WarehouseName);

public record AccessUserResponse(
    Guid Id,
    string Username,
    string Email,
    string DisplayName,
    string Role,
    bool IsActive,
    IReadOnlyList<GrantResponse> Grants,
    IReadOnlyList<AssignedWarehouseResponse> AssignedWarehouses);

public record AssignRoleRequest([param: Required] Guid RoleId);

public record GrantPermissionRequest(
    [param: Required] Guid PermissionId,
    Guid? WarehouseId = null);