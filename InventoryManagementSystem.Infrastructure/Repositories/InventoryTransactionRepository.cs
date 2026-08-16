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
}