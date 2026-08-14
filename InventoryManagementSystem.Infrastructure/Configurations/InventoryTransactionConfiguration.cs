using InventoryManagementSystem.Domain.Entities;
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
    }
}