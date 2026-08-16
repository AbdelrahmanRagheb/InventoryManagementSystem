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

        builder.ToTable(t =>
            t.HasCheckConstraint(
                "CK_Inventory_Quantity_NonNegative",
                "[Quantity] >= 0"));

        builder.HasOne(i => i.Product)
            .WithMany(p => p.Inventories)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Warehouse)
            .WithMany(w => w.Inventories)
            .HasForeignKey(i => i.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        var seedDate = new DateTime(
            2026,
            1,
            15,
            8,
            0,
            0,
            DateTimeKind.Utc);

        builder.HasData(
            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000001"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000001"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Quantity = 50,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000002"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000002"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"),
                Quantity = 25,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000003"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000003"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Quantity = 12,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000004"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000004"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Quantity = 45,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000005"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000005"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Quantity = 18,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000006"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000006"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Quantity = 55,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000007"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000007"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"),
                Quantity = 30,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000008"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000008"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Quantity = 20,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000009"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000009"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Quantity = 50,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000010"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000010"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"),
                Quantity = 15,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000011"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000011"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Quantity = 50,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000012"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000012"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Quantity = 15,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000013"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000013"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Quantity = 11,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000014"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000014"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Quantity = 55,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000015"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000015"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Quantity = 22,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000016"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000016"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Quantity = 47,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000017"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000017"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"),
                Quantity = 28,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000018"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000018"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Quantity = 22,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000019"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000019"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Quantity = 47,
                UpdatedAt = seedDate
            },

            new Inventory
            {
                Id = new Guid("50000000-0000-0000-0000-000000000020"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000020"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"),
                Quantity = 32,
                UpdatedAt = seedDate
            }
        );
    }
}