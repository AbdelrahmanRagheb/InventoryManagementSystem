using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Application.DTOs.Products;

public record CreateProductRequest(
    [param: Required, StringLength(150)] string Name,
    [param: Required] Guid CategoryId,
    [param: Range(0, double.MaxValue)] decimal Price,
    bool IsActive);

public record UpdateProductRequest(
    [param: StringLength(150)] string? Name = null,
    Guid? CategoryId = null,
    [param: Range(0, double.MaxValue)] decimal? Price = null,
    bool? IsActive = null);

public record ProductResponse(Guid Id, string Name, Guid? CategoryId, decimal Price, bool IsActive, DateTime CreatedAt);