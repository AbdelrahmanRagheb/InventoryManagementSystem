using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task SaveChangesAsync();
}