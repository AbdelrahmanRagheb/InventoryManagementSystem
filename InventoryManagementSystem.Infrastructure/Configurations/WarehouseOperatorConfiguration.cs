using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Configurations;

public class WarehouseOperatorConfiguration : IEntityTypeConfiguration<WarehouseOperator>
{
    public void Configure(EntityTypeBuilder<WarehouseOperator> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasIndex(o => new { o.WarehouseId, o.OperatorUserId })
            .IsUnique();

        builder.HasOne(o => o.Warehouse)
            .WithMany(w => w.Operators)
            .HasForeignKey(o => o.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(o => o.OperatorUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}