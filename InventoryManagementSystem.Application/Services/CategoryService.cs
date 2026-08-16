using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class CategoryService : BaseService<Category>, ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;
    public CategoryService(ICategoryRepository repo) : base(repo) => _categoryRepo = repo;

    public async Task UpdateAsync(Category category) { await _categoryRepo.UpdateAsync(category); }

    public override async Task DeleteAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category != null) { category.IsActive = false; await _categoryRepo.UpdateAsync(category); }
    }
}