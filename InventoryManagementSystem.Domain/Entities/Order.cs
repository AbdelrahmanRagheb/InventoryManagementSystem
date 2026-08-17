using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerEmail { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }

    public User CreatedBy { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ICollection<OrderHistory> History { get; set; } = new List<OrderHistory>();
}