using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task UpdateAsync(Product entity);
}
