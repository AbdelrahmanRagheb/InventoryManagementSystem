using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public class UserPermissionRepository : BaseRepository<UserPermission>, IUserPermissionRepository
{
    public UserPermissionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<UserPermission>> GetByUserAsync(Guid userId) =>
        await _context.Set<UserPermission>()
            .Include(up => up.Permission)
            .Include(up => up.Resource)
            .Where(up => up.UserId == userId)
            .ToListAsync();

    public async Task<UserPermission?> GetAsync(Guid userId, Guid permissionId, Guid? resourceId) =>
        await _context.Set<UserPermission>()
            .FirstOrDefaultAsync(up =>
                up.UserId == userId &&
                up.PermissionId == permissionId &&
                up.ResourceId == resourceId);

    public async Task RemoveAsync(Guid id)
    {
        var entity = await _context.Set<UserPermission>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<UserPermission>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ReplaceForUserAsync(Guid userId, IEnumerable<UserPermission> permissions)
    {
        var existing = await _context.Set<UserPermission>().Where(up => up.UserId == userId).ToListAsync();
        _context.Set<UserPermission>().RemoveRange(existing);
        _context.Set<UserPermission>().AddRange(permissions);
        await _context.SaveChangesAsync();
    }
}