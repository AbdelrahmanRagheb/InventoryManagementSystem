using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IService<TEntity> where TEntity : class
{
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    Task DeleteAsync(Guid id);
}