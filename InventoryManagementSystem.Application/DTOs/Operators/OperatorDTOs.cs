namespace InventoryManagementSystem.Application.DTOs.Operators;

public record AssignOperatorRequest(Guid WarehouseId, Guid OperatorUserId);

public record UpdateOperatorRequest(Guid? WarehouseId = null, Guid? OperatorUserId = null);

public record OperatorResponse(Guid Id, Guid WarehouseId, Guid OperatorUserId);