using InventoryManagementSystem.Application.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public abstract class BaseService<TEntity> : IService<TEntity> where TEntity : class
{
    protected readonly IRepository<TEntity> _repo;
    protected BaseService(IRepository<TEntity> repo) => _repo = repo;

    public virtual Task<IReadOnlyList<TEntity>> GetAllAsync() => _repo.GetAllAsync();
    public virtual Task<TEntity?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);
    public virtual Task AddAsync(TEntity entity) => _repo.AddAsync(entity);
    public abstract Task DeleteAsync(Guid id);
}