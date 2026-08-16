using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IProductService : IService<Product>
{
    Task UpdateAsync(Product product);
}