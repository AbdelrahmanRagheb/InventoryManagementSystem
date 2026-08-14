using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Configurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.HasKey(i => i.Id);

        builder.HasIndex(i => new { i.ProductId, i.WarehouseId })
            .IsUnique();

        builder.ToTable(t => t.HasCheckConstraint("CK_Inventory_Quantity_NonNegative", "[Quantity] >= 0"));

        builder.HasOne(i => i.Product)
            .WithMany(p => p.Inventories)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Warehouse)
            .WithMany(w => w.Inventories)
            .HasForeignKey(i => i.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new Inventory { Id = new Guid("50000000-0000-0000-0000-000000000001"), ProductId = new Guid("30000000-0000-0000-0000-000000000001"), WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"), Quantity = 50, UpdatedAt = new DateTime(2026, 2, 20, 14, 15, 0, DateTimeKind.Utc) },
            new Inventory { Id = new Guid("50000000-0000-0000-0000-000000000002"), ProductId = new Guid("30000000-0000-0000-0000-000000000001"), WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"), Quantity = 20, UpdatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc) },
            new Inventory { Id = new Guid("50000000-0000-0000-0000-000000000003"), ProductId = new Guid("30000000-0000-0000-0000-000000000001"), WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"), Quantity = 0, UpdatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc) },
            new Inventory { Id = new Guid("50000000-0000-0000-0000-000000000004"), ProductId = new Guid("30000000-0000-0000-0000-000000000002"), WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"), Quantity = 15, UpdatedAt = new DateTime(2026, 1, 15, 8, 5, 0, DateTimeKind.Utc) },
            new Inventory { Id = new Guid("50000000-0000-0000-0000-000000000005"), ProductId = new Guid("30000000-0000-0000-0000-000000000003"), WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"), Quantity = 8, UpdatedAt = new DateTime(2026, 1, 20, 9, 0, 0, DateTimeKind.Utc) },
            new Inventory { Id = new Guid("50000000-0000-0000-0000-000000000006"), ProductId = new Guid("30000000-0000-0000-0000-000000000005"), WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"), Quantity = 100, UpdatedAt = new DateTime(2026, 1, 15, 8, 10, 0, DateTimeKind.Utc) }
        );
    }
}