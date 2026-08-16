namespace InventoryManagementSystem.Application.DTOs.Products;

public record CreateProductRequest(string Name, Guid CategoryId, bool IsActive);

public record UpdateProductRequest(string? Name = null, Guid? CategoryId = null, bool? IsActive = null);

public record ProductResponse(Guid Id, string Name, Guid? CategoryId, bool IsActive, DateTime CreatedAt);