using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IProductService : IService<Product>
{
    Task UpdateAsync(Product product);
    Task<Product?> ActivateAsync(Guid id);
    Task<PagedResponse<Product>> GetPagedAsync(int page, int pageSize);
}