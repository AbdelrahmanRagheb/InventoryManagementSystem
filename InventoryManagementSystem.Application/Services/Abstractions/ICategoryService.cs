using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface ICategoryService : IService<Category>
{
    Task UpdateAsync(Category category);
    Task<Category?> ActivateAsync(Guid id);
    Task<bool> HardDeleteAsync(Guid id);
    Task<PagedResponse<Category>> GetPagedAsync(int page, int pageSize);
}