using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task UpdateAsync(Category entity);
    Task RemoveAsync(Category entity);
}
