using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public class ResourceRepository : BaseRepository<Resource>, IResourceRepository
{
    public ResourceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task UpdateAsync(Resource resource)
    {
        _context.Set<Resource>().Update(resource);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Resource>> GetByTypeAsync(string type) =>
        await _context.Set<Resource>().Where(r => r.Type == type).ToListAsync();
}