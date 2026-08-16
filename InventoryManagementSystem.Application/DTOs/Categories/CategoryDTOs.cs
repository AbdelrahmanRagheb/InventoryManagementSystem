namespace InventoryManagementSystem.Application.DTOs.Categories;

public record CreateCategoryRequest(string Name, string? Description);

public record UpdateCategoryRequest(string? Name = null, string? Description = null);

public record CategoryResponse(Guid Id, string Name, string? Description, bool IsActive, DateTime CreatedAt);