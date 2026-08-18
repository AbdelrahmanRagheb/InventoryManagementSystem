using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IUserPermissionRepository : IRepository<UserPermission>
{
    Task<IReadOnlyList<UserPermission>> GetByUserAsync(Guid userId);
    Task<UserPermission?> GetAsync(Guid userId, Guid permissionId, Guid? resourceId);
    Task RemoveAsync(Guid id);
    Task ReplaceForUserAsync(Guid userId, IEnumerable<UserPermission> permissions);
}