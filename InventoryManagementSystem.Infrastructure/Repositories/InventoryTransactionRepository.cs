using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public class InventoryTransactionRepository : BaseRepository<InventoryTransaction>, IInventoryTransactionRepository
{
    public InventoryTransactionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<InventoryTransaction>> GetByWarehouseAsync(Guid warehouseId) =>
        await _context.Set<InventoryTransaction>()
            .Where(t => t.WarehouseId == warehouseId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

    public async Task<IReadOnlyList<InventoryTransaction>> GetByProductAsync(Guid productId) =>
        await _context.Set<InventoryTransaction>()
            .Where(t => t.ProductId == productId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

    public async Task<(IReadOnlyList<InventoryTransaction> Items, int Total)> GetPagedAsync(int page, int pageSize, IReadOnlyList<Guid>? warehouseIds)
    {
        var query = _context.Set<InventoryTransaction>().AsQueryable();
        if (warehouseIds != null)
            query = query.Where(t => warehouseIds.Contains(t.WarehouseId));
        query = query.OrderByDescending(t => t.CreatedAt);
        return await ApplyPagingAsync(query, page, pageSize);
    }

    public async Task<(IReadOnlyList<InventoryTransaction> Items, int Total)> GetByProductPagedAsync(Guid productId, int page, int pageSize, IReadOnlyList<Guid>? warehouseIds)
    {
        var query = _context.Set<InventoryTransaction>().Where(t => t.ProductId == productId).AsQueryable();
        if (warehouseIds != null)
            query = query.Where(t => warehouseIds.Contains(t.WarehouseId));
        query = query.OrderByDescending(t => t.CreatedAt);
        return await ApplyPagingAsync(query, page, pageSize);
    }

    public async Task<(IReadOnlyList<InventoryTransaction> Items, int Total)> GetByWarehousePagedAsync(Guid warehouseId, int page, int pageSize)
    {
        var query = _context.Set<InventoryTransaction>()
            .Where(t => t.WarehouseId == warehouseId)
            .OrderByDescending(t => t.CreatedAt);
        return await ApplyPagingAsync(query, page, pageSize);
    }
}