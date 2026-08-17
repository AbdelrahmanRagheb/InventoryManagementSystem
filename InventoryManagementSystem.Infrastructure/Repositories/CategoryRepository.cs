using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task UpdateAsync(Category entity)
    {
        _context.Set<Category>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Category entity)
    {
        _context.Set<Category>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}