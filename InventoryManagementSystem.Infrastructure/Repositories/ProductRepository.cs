using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task UpdateAsync(Product entity)
    {
        _context.Set<Product>().Update(entity);
        await _context.SaveChangesAsync();
    }
}