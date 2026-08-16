using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Configurations;

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Reason)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(t => t.Product)
            .WithMany()
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Warehouse)
            .WithMany()
            .HasForeignKey(t => t.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.CreatedAt);

        builder.HasData(
            // Transaction 1
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000001"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000001"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Type = TransactionType.Initial,
                QuantityChange = 50,
                PreviousQuantity = 0,
                NewQuantity = 50,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 2
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000002"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000002"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"),
                Type = TransactionType.Initial,
                QuantityChange = 25,
                PreviousQuantity = 0,
                NewQuantity = 25,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 3
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000003"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000003"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Type = TransactionType.Initial,
                QuantityChange = 12,
                PreviousQuantity = 0,
                NewQuantity = 12,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 4
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000004"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000004"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Type = TransactionType.Initial,
                QuantityChange = 45,
                PreviousQuantity = 0,
                NewQuantity = 45,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 5
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000005"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000005"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Type = TransactionType.Initial,
                QuantityChange = 18,
                PreviousQuantity = 0,
                NewQuantity = 18,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 6
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000006"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000006"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Type = TransactionType.Initial,
                QuantityChange = 55,
                PreviousQuantity = 0,
                NewQuantity = 55,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 7
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000007"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000007"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"),
                Type = TransactionType.Initial,
                QuantityChange = 30,
                PreviousQuantity = 0,
                NewQuantity = 30,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 8
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000008"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000008"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Type = TransactionType.Initial,
                QuantityChange = 20,
                PreviousQuantity = 0,
                NewQuantity = 20,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 9
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000009"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000009"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Type = TransactionType.Initial,
                QuantityChange = 50,
                PreviousQuantity = 0,
                NewQuantity = 50,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 10
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000010"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000010"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"),
                Type = TransactionType.Initial,
                QuantityChange = 30,
                PreviousQuantity = 0,
                NewQuantity = 30,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 11
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000011"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000011"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Type = TransactionType.Initial,
                QuantityChange = 50,
                PreviousQuantity = 0,
                NewQuantity = 50,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 12
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000012"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000012"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Type = TransactionType.Initial,
                QuantityChange = 15,
                PreviousQuantity = 0,
                NewQuantity = 15,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 13
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000013"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000013"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Type = TransactionType.Initial,
                QuantityChange = 11,
                PreviousQuantity = 0,
                NewQuantity = 11,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 14
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000014"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000014"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Type = TransactionType.Initial,
                QuantityChange = 55,
                PreviousQuantity = 0,
                NewQuantity = 55,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 15
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000015"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000015"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Type = TransactionType.Initial,
                QuantityChange = 22,
                PreviousQuantity = 0,
                NewQuantity = 22,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 16
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000016"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000016"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Type = TransactionType.Initial,
                QuantityChange = 47,
                PreviousQuantity = 0,
                NewQuantity = 47,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 17
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000017"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000017"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"),
                Type = TransactionType.Initial,
                QuantityChange = 28,
                PreviousQuantity = 0,
                NewQuantity = 28,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 18
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000018"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000018"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"),
                Type = TransactionType.Initial,
                QuantityChange = 22,
                PreviousQuantity = 0,
                NewQuantity = 22,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 19
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000019"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000019"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"),
                Type = TransactionType.Initial,
                QuantityChange = 47,
                PreviousQuantity = 0,
                NewQuantity = 47,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },

            // Transaction 20
            new InventoryTransaction
            {
                Id = new Guid("70000000-0000-0000-0000-000000000020"),
                ProductId = new Guid("30000000-0000-0000-0000-000000000020"),
                WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"),
                Type = TransactionType.Initial,
                QuantityChange = 32,
                PreviousQuantity = 0,
                NewQuantity = 32,
                Reason = "Initial stock on setup",
                CreatedByUserId = new Guid("40000000-0000-0000-0000-000000000001"),
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}