using System.ComponentModel.DataAnnotations;
using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Application.DTOs.Inventory;

public record CreateInventoryRequest(
    [param: Required] Guid ProductId,
    [param: Required] Guid WarehouseId,
    [param: Range(0, int.MaxValue)] int Quantity,
    [param: Required, StringLength(500)] string Reason);

public record AdjustStockRequest(
    [param: Required] Guid ProductId,
    [param: Required] Guid WarehouseId,
    [param: Required] TransactionType Type,
    [param: Range(1, int.MaxValue)] int QuantityChange,
    [param: Required, StringLength(500)] string Reason);

public record InventoryResponse(Guid Id, Guid ProductId, Guid WarehouseId, int Quantity, DateTime UpdatedAt);

public record InventoryTransactionResponse(Guid Id, Guid ProductId, Guid WarehouseId, int Type, int QuantityChange, int PreviousQuantity, int NewQuantity, string Reason, Guid CreatedByUserId, DateTime CreatedAt);