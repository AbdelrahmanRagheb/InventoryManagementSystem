using InventoryManagementSystem.Application.DTOs.Categories;
using InventoryManagementSystem.Application.Services;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService) => _categoryService = categoryService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories.Select(ToResponse));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(ToResponse(category));
    }

    [HttpPost]
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
    public async Task<IActionResult> DeactivateCategory(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound(new { message = $"Category with id: {id} does not exist" });
        await _categoryService.DeactivateAsync(id);
        return Ok(ToResponse(category));
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var category = await _categoryService.ActivateAsync(id);
        if (category == null) return NotFound(new { message = $"Category with id: {id} does not exist" });
        return Ok(ToResponse(category));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> HardDelete(Guid id)
    {
        var deleted = await _categoryService.HardDeleteAsync(id);
        if (!deleted) return NotFound(new { message = $"Category with id: {id} does not exist" });
        return Ok(new { message = $"Category with id: {id} is deleted successfully" });
    }

    private static CategoryResponse ToResponse(Category category) =>
        new(category.Id, category.Name, category.Description, category.IsActive, category.CreatedAt);
}