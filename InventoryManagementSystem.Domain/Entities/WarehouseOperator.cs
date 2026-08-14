namespace InventoryManagementSystem.Domain.Entities;

public class WarehouseOperator
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid OperatorUserId { get; set; }

    public Warehouse Warehouse { get; set; } = null!;
}