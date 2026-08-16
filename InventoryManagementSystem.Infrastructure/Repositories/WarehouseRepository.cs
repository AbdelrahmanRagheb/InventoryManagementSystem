using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(AppDbContext context) : base(context)
    {
    }

    public async Task UpdateAsync(Warehouse entity)
    {
        _context.Set<Warehouse>().Update(entity);
        await _context.SaveChangesAsync();
    }
}