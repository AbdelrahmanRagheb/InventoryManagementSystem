namespace InventoryManagementSystem.Application.DTOs.Warehouses;

public record CreateWarehouseRequest(string Name, string? Location);

public record UpdateWarehouseRequest(string? Name = null, string? Location = null);

public record WarehouseResponse(Guid Id, string Name, string? Location, bool IsActive, DateTime CreatedAt);