using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Application.DTOs.Products;

public record CreateProductRequest(
    [param: Required, StringLength(150)] string Name,
    [param: Required] Guid CategoryId,
    bool IsActive);

public record UpdateProductRequest(
    [param: StringLength(150)] string? Name = null,
    Guid? CategoryId = null,
    bool? IsActive = null);

public record ProductResponse(Guid Id, string Name, Guid? CategoryId, bool IsActive, DateTime CreatedAt);