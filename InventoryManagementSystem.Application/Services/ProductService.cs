using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class ProductService : BaseService<Product>, IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;

    public ProductService(IProductRepository productRepo, ICategoryRepository categoryRepo) : base(productRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task UpdateAsync(Product product)
    {
        if (product.CategoryId is Guid categoryId && categoryId != Guid.Empty)
        {
            var cat = await _categoryRepo.GetByIdAsync(categoryId);
            if (cat == null || !cat.IsActive) throw new InvalidOperationException("Invalid or inactive category");
        }
        await _productRepo.UpdateAsync(product);
    }

    public override async Task DeactivateAsync(Guid id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product != null) { product.IsActive = false; await _productRepo.UpdateAsync(product); }
    }

    public async Task<Product?> ActivateAsync(Guid id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null) return null;
        product.IsActive = true;
        await _productRepo.UpdateAsync(product);
        return product;
    }
}