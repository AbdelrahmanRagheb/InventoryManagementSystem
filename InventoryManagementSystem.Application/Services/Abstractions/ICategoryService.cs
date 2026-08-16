using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface ICategoryService : IService<Category>
{
    Task UpdateAsync(Category category);
}