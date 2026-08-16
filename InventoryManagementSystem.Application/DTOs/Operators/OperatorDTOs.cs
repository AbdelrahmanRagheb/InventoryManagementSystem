using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Application.DTOs.Operators;

public record AssignOperatorRequest(
    [param: Required] Guid WarehouseId,
    [param: Required] Guid OperatorUserId);

public record UpdateOperatorRequest(
    Guid? WarehouseId = null,
    Guid? OperatorUserId = null);

public record OperatorResponse(Guid Id, Guid WarehouseId, Guid OperatorUserId);