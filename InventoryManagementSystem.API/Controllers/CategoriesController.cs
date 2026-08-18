using InventoryManagementSystem.Application.DTOs.Categories;
using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Application.Services;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService) => _categoryService = categoryService;

    [HttpGet]
    [Authorize(Policy = "Category.View")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = Paging.DefaultPageSize)
    {
        var categories = await _categoryService.GetPagedAsync(page, pageSize);
        return Ok(new PagedResponse<CategoryResponse>(
            categories.Items.Select(ToResponse).ToList(),
            categories.Page,
            categories.PageSize,
            categories.TotalCount));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "Category.View")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(ToResponse(category));
    }

    [HttpPost]
    [Authorize(Policy = "Category.Create")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _categoryService.AddAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, ToResponse(category));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Category.Edit")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();
        if (request.Name != null) category.Name = request.Name;
        if (request.Description != null) category.Description = request.Description;
        await _categoryService.UpdateAsync(category);
        return Ok(ToResponse(category));
    }

    [HttpPost("{id:guid}/deactivate")]
    [Authorize(Policy = "Category.Deactivate")]
    public async Task<IActionResult> DeactivateCategory(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound(new { message = $"Category with id: {id} does not exist" });
        await _categoryService.DeactivateAsync(id);
        return Ok(ToResponse(category));
    }

    [HttpPost("{id:guid}/activate")]
    [Authorize(Policy = "Category.Activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var category = await _categoryService.ActivateAsync(id);
        if (category == null) return NotFound(new { message = $"Category with id: {id} does not exist" });
        return Ok(ToResponse(category));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Category.Delete")]
    public async Task<IActionResult> HardDelete(Guid id)
    {
        var deleted = await _categoryService.HardDeleteAsync(id);
        if (!deleted) return NotFound(new { message = $"Category with id: {id} does not exist" });
        return Ok(new { message = $"Category with id: {id} is deleted successfully" });
    }

    private static CategoryResponse ToResponse(Category category) =>
        new(category.Id, category.Name, category.Description, category.IsActive, category.CreatedAt);
}