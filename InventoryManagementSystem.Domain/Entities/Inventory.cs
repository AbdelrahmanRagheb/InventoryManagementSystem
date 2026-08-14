namespace InventoryManagementSystem.Domain.Entities;

public class Inventory
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
    public ICollection<InventoryTransaction> Transactions { get; set; } = new List<InventoryTransaction>();
}