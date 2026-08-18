using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<(IReadOnlyList<Warehouse> Items, int Total)> GetPagedAsync(int page, int pageSize, IReadOnlyList<Guid>? warehouseIds)
    {
        var query = _context.Set<Warehouse>().AsQueryable();
        if (warehouseIds != null)
            query = query.Where(w => warehouseIds.Contains(w.Id));
        return await ApplyPagingAsync(query, page, pageSize);
    }

    public async Task UpdateAsync(Warehouse entity)
    {
        _context.Set<Warehouse>().Update(entity);
        await _context.SaveChangesAsync();
    }
}