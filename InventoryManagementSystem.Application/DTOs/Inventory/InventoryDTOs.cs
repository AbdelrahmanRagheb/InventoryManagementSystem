namespace InventoryManagementSystem.Application.DTOs.Inventory;

public record CreateInventoryRequest(Guid ProductId, Guid WarehouseId, int Quantity, string Reason);

public record AdjustStockRequest(Guid ProductId, Guid WarehouseId, int QuantityChange, string Reason);

public record InventoryResponse(Guid Id, Guid ProductId, Guid WarehouseId, int Quantity, DateTime UpdatedAt);

public record InventoryTransactionResponse(Guid Id, Guid ProductId, Guid WarehouseId, int Type, int QuantityChange, int PreviousQuantity, int NewQuantity, string Reason, Guid CreatedByUserId, DateTime CreatedAt);