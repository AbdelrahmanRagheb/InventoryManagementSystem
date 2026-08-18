using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
{
    public InventoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Inventory?> GetByProductWarehouseAsync(Guid productId, Guid warehouseId) =>
        await _context.Set<Inventory>().FirstOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);

    public async Task<IReadOnlyList<Inventory>> GetByProductAsync(Guid productId) =>
        await _context.Set<Inventory>().Where(i => i.ProductId == productId).ToListAsync();

    public async Task<IReadOnlyList<Inventory>> GetByWarehouseAsync(Guid warehouseId) =>
        await _context.Set<Inventory>().Where(i => i.WarehouseId == warehouseId).ToListAsync();

    public async Task<(IReadOnlyList<Inventory> Items, int Total)> GetPagedAsync(int page, int pageSize, IReadOnlyList<Guid>? warehouseIds)
    {
        var query = _context.Set<Inventory>().AsQueryable();
        if (warehouseIds != null)
            query = query.Where(i => warehouseIds.Contains(i.WarehouseId));
        return await ApplyPagingAsync(query, page, pageSize);
    }

    public async Task<(IReadOnlyList<Inventory> Items, int Total)> GetByProductPagedAsync(Guid productId, int page, int pageSize, IReadOnlyList<Guid>? warehouseIds)
    {
        var query = _context.Set<Inventory>().Where(i => i.ProductId == productId).AsQueryable();
        if (warehouseIds != null)
            query = query.Where(i => warehouseIds.Contains(i.WarehouseId));
        return await ApplyPagingAsync(query, page, pageSize);
    }

    public async Task<(IReadOnlyList<Inventory> Items, int Total)> GetByWarehousePagedAsync(Guid warehouseId, int page, int pageSize)
    {
        var query = _context.Set<Inventory>().Where(i => i.WarehouseId == warehouseId);
        return await ApplyPagingAsync(query, page, pageSize);
    }

    public async Task UpdateAsync(Inventory entity)
    {
        _context.Set<Inventory>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var entity = await _context.Set<Inventory>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<Inventory>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}