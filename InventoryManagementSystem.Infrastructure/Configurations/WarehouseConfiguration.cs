using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(w => w.Location)
            .HasMaxLength(300);

        builder.HasData(
            new Warehouse { Id = new Guid("20000000-0000-0000-0000-000000000001"), Name = "Main Warehouse", Location = "Downtown, Cairo", IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Warehouse { Id = new Guid("20000000-0000-0000-0000-000000000002"), Name = "Alexandria Warehouse", Location = "Alexandria", IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Warehouse { Id = new Guid("20000000-0000-0000-0000-000000000003"), Name = "Cairo Warehouse", Location = "Nasr City, Cairo", IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}