using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Order?> GetByIdWithItemsAsync(Guid id) =>
        await _context.Set<Order>().Include(o => o.Items).ThenInclude(i => i.Product).Include(o => o.History).FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IReadOnlyList<Order>> GetAllWithItemsAsync() =>
        await _context.Set<Order>().Include(o => o.Items).ThenInclude(i => i.Product).Include(o => o.History).ToListAsync();

    public async Task<IReadOnlyList<Order>> GetByUserAsync(Guid userId) =>
        await _context.Set<Order>().Where(o => o.CreatedByUserId == userId).Include(o => o.Items).ThenInclude(i => i.Product).Include(o => o.History).ToListAsync();

    public async Task UpdateAsync(Order entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            _context.Set<Order>().Update(entity);
        await _context.SaveChangesAsync();
    }
}