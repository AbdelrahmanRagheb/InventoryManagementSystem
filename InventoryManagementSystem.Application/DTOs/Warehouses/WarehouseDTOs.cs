using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Application.DTOs.Warehouses;

public record CreateWarehouseRequest(
    [param: Required, StringLength(150)] string Name,
    [param: StringLength(300)] string? Location);

public record UpdateWarehouseRequest(
    [param: StringLength(150)] string? Name = null,
    [param: StringLength(300)] string? Location = null);

public record WarehouseResponse(Guid Id, string Name, string? Location, bool IsActive, DateTime CreatedAt);