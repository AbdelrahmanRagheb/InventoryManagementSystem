using InventoryManagementSystem.Application.Repositories;
using System.Collections.Generic;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task UpdateAsync(Product entity);
    Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<IReadOnlyList<Product>> GetByCategoryAsync(Guid categoryId);
    Task<(IReadOnlyList<Product> Items, int Total)> GetPagedAsync(int page, int pageSize);
}
