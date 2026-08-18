namespace InventoryManagementSystem.Domain.Entities;

public class Warehouse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid? ResourceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Resource? Resource { get; set; }
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    public ICollection<WarehouseOperator> Operators { get; set; } = new List<WarehouseOperator>();
}