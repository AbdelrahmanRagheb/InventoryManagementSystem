using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public class WarehouseOperatorRepository : BaseRepository<WarehouseOperator>, IWarehouseOperatorRepository
{
    public WarehouseOperatorRepository(AppDbContext context) : base(context)
    {
    }

    public async Task UpdateAsync(WarehouseOperator entity)
    {
        _context.Set<WarehouseOperator>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public override async Task<IReadOnlyList<WarehouseOperator>> GetAllAsync() =>
        await _context.Set<WarehouseOperator>().Include(o => o.Operator).ToListAsync();

    public async Task<WarehouseOperator?> GetByWarehouseAndUserAsync(Guid warehouseId, Guid userId) =>
        await _context.Set<WarehouseOperator>().Include(o => o.Operator).FirstOrDefaultAsync(wo => wo.WarehouseId == warehouseId && wo.OperatorUserId == userId);

    public async Task<IReadOnlyList<WarehouseOperator>> GetByWarehouseAsync(Guid warehouseId) =>
        await _context.Set<WarehouseOperator>().Include(o => o.Operator).Where(wo => wo.WarehouseId == warehouseId).ToListAsync();

    public async Task<IReadOnlyList<WarehouseOperator>> GetByOperatorAsync(Guid userId) =>
        await _context.Set<WarehouseOperator>().Include(o => o.Operator).Where(wo => wo.OperatorUserId == userId).ToListAsync();

    public async Task RemoveAsync(Guid warehouseId, Guid userId)
    {
        var entity = await _context.Set<WarehouseOperator>().FirstOrDefaultAsync(wo => wo.WarehouseId == warehouseId && wo.OperatorUserId == userId);
        if (entity != null)
        {
            _context.Set<WarehouseOperator>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}