namespace InventoryManagementSystem.Domain.Entities;

public class UserPermission
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PermissionId { get; set; }
    public Guid? ResourceId { get; set; }

    public User User { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
    public Resource? Resource { get; set; }
}