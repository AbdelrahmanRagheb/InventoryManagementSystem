using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(r => new { r.Type, r.Name })
            .IsUnique();

        builder.HasData(
            new Resource { Id = new Guid("70000000-0000-0000-0000-000000000001"), Type = "Warehouse", Name = "Main Warehouse" },
            new Resource { Id = new Guid("70000000-0000-0000-0000-000000000002"), Type = "Warehouse", Name = "Alexandria Warehouse" },
            new Resource { Id = new Guid("70000000-0000-0000-0000-000000000003"), Type = "Warehouse", Name = "Cairo Warehouse" }
        );
    }
}