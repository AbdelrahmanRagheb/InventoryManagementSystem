using InventoryManagementSystem.Application.Repositories;
using System.Collections.Generic;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task UpdateAsync(Category entity);
    Task RemoveAsync(Category entity);
    Task<(IReadOnlyList<Category> Items, int Total)> GetPagedAsync(int page, int pageSize);
}
