using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Domain.Entities;

public class InventoryTransaction
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public TransactionType Type { get; set; }
    public int QuantityChange { get; set; }
    public int PreviousQuantity { get; set; }
    public int NewQuantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
}