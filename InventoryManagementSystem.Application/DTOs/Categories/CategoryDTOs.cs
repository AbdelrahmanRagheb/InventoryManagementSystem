using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Application.DTOs.Categories;

public record CreateCategoryRequest(
    [param: Required, StringLength(100)] string Name,
    [param: StringLength(500)] string? Description);

public record UpdateCategoryRequest(
    [param: StringLength(100)] string? Name = null,
    [param: StringLength(500)] string? Description = null);

public record CategoryResponse(Guid Id, string Name, string? Description, bool IsActive, DateTime CreatedAt);