using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task UpdateAsync(Product entity);
    Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<Guid> ids);
}
