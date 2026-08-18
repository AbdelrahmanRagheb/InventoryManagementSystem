using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class CategoryService : BaseService<Category>, ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly IProductRepository _productRepo;

    public CategoryService(ICategoryRepository repo, IProductRepository productRepo) : base(repo)
    {
        _categoryRepo = repo;
        _productRepo = productRepo;
    }

    public async Task<PagedResponse<Category>> GetPagedAsync(int page, int pageSize)
    {
        var (p, ps) = Paging.Normalize(page, pageSize);
        var (items, total) = await _categoryRepo.GetPagedAsync(p, ps);
        return new PagedResponse<Category>(items, p, ps, total);
    }

    public async Task UpdateAsync(Category category) { await _categoryRepo.UpdateAsync(category); }

    public async Task<Category?> ActivateAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null) return null;
        category.IsActive = true;
        await _categoryRepo.UpdateAsync(category);
        await CascadeProductsAsync(category.Id, active: true);
        return category;
    }

    public override async Task DeactivateAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category != null)
        {
            category.IsActive = false;
            await _categoryRepo.UpdateAsync(category);
            await CascadeProductsAsync(category.Id, active: false);
        }
    }

    public async Task<bool> HardDeleteAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null) return false;
        await _categoryRepo.RemoveAsync(category);
        return true;
    }

    private async Task CascadeProductsAsync(Guid categoryId, bool active)
    {
        var products = await _productRepo.GetByCategoryAsync(categoryId);
        foreach (var product in products)
        {
            product.IsActive = active;
            await _productRepo.UpdateAsync(product);
        }
    }
}