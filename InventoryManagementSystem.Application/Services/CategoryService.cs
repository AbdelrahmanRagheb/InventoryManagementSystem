using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class CategoryService : BaseService<Category>, ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;
    public CategoryService(ICategoryRepository repo) : base(repo) => _categoryRepo = repo;

    public async Task UpdateAsync(Category category) { await _categoryRepo.UpdateAsync(category); }

    public async Task<Category?> ActivateAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null) return null;
        category.IsActive = true;
        await _categoryRepo.UpdateAsync(category);
        return category;
    }

    public override async Task DeactivateAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category != null) { category.IsActive = false; await _categoryRepo.UpdateAsync(category); }
    }

    public async Task<bool> HardDeleteAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null) return false;
        await _categoryRepo.RemoveAsync(category);
        return true;
    }
}