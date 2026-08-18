using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Configurations;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.HasKey(up => up.Id);

        builder.HasIndex(up => up.UserId);

        builder.HasIndex(up => new { up.UserId, up.PermissionId, up.ResourceId })
            .IsUnique()
            .HasFilter("[ResourceId] IS NOT NULL");

        builder.HasIndex(up => new { up.UserId, up.PermissionId })
            .IsUnique()
            .HasFilter("[ResourceId] IS NULL");

        builder.HasOne(up => up.User)
            .WithMany(u => u.Permissions)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(up => up.Permission)
            .WithMany(p => p.UserPermissions)
            .HasForeignKey(up => up.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(up => up.Resource)
            .WithMany(r => r.UserPermissions)
            .HasForeignKey(up => up.ResourceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(Seed());
    }

    private static IEnumerable<UserPermission> Seed()
    {
        Guid P(string id) => new($"60000000-0000-0000-0000-{id}");
        Guid U(string id) => new($"40000000-0000-0000-0000-{id}");
        Guid UP(string id) => new($"80000000-0000-0000-0000-{id}");

        var permissions = new Dictionary<string, Guid>
        {
            ["Product.View"] = P("000000000008"),
            ["Category.View"] = P("000000000013"),
            ["Warehouse.View"] = P("000000000019"),
            ["Inventory.View"] = P("000000000022"),
            ["Inventory.Adjust"] = P("000000000023"),
            ["Order.Create"] = P("000000000024"),
            ["Order.Edit"] = P("000000000025"),
            ["Order.Cancel"] = P("000000000026"),
            ["Order.Complete"] = P("000000000027"),
            ["Order.View"] = P("000000000028"),
            ["Order.ViewOwn"] = P("000000000029"),
            ["OrderItem.Add"] = P("000000000030"),
            ["OrderItem.Remove"] = P("000000000031"),
            ["Report.ViewOrders"] = P("000000000032"),
            ["Report.ViewInventory"] = P("000000000033"),
            ["Report.ViewTransactions"] = P("000000000034"),
            ["Report.ViewWarehouseSummary"] = P("000000000035")
        };

        var rows = new List<(string User, string[] PermissionNames)>
        {
            ("000000000003", new[] { "Product.View", "Warehouse.View", "Inventory.View", "Inventory.Adjust", "Order.View", "Order.Complete" }),
            ("000000000004", new[] { "Product.View", "Warehouse.View", "Inventory.View", "Inventory.Adjust", "Order.View", "Order.Complete" }),
            ("000000000005", new[] { "Product.View", "Warehouse.View", "Inventory.View", "Inventory.Adjust", "Order.View", "Order.Complete" }),
            ("000000000006", new[] { "Category.View", "Product.View", "Warehouse.View", "Inventory.View", "Order.View", "Report.ViewOrders", "Report.ViewInventory", "Report.ViewTransactions", "Report.ViewWarehouseSummary" }),
            ("000000000007", new[] { "Product.View", "Order.Create", "Order.Edit", "Order.Cancel", "Order.ViewOwn", "OrderItem.Add", "OrderItem.Remove" })
        };

        var seed = new List<UserPermission>();
        foreach (var (user, names) in rows)
        {
            foreach (var name in names)
            {
                seed.Add(new UserPermission
                {
                    Id = UP(seed.Count.ToString().PadLeft(12, '0')),
                    UserId = U(user),
                    PermissionId = permissions[name],
                    ResourceId = null
                });
            }
        }
        return seed;
    }
}