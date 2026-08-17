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

        builder.HasOne(o => o.Operator)
            .WithMany()
            .HasForeignKey(o => o.OperatorUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new WarehouseOperator { Id = new Guid("60000000-0000-0000-0000-000000000001"), WarehouseId = new Guid("20000000-0000-0000-0000-000000000001"), OperatorUserId = new Guid("40000000-0000-0000-0000-000000000003") },
            new WarehouseOperator { Id = new Guid("60000000-0000-0000-0000-000000000002"), WarehouseId = new Guid("20000000-0000-0000-0000-000000000002"), OperatorUserId = new Guid("40000000-0000-0000-0000-000000000004") },
            new WarehouseOperator { Id = new Guid("60000000-0000-0000-0000-000000000003"), WarehouseId = new Guid("20000000-0000-0000-0000-000000000003"), OperatorUserId = new Guid("40000000-0000-0000-0000-000000000005") }
        );
    }
}