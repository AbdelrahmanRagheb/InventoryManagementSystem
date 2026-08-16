using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;

    protected BaseRepository(AppDbContext context) => _context = context;

    public virtual async Task<TEntity?> GetByIdAsync(Guid id) => await _context.Set<TEntity>().FindAsync(id);

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync() => await _context.Set<TEntity>().ToListAsync();

    public virtual async Task AddAsync(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public virtual Task SaveChangesAsync() => _context.SaveChangesAsync();
}