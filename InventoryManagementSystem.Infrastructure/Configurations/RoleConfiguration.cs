using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(r => r.Name)
            .IsUnique();

        builder.HasData(
            new Role { Id = new Guid("50000000-0000-0000-0000-000000000001"), Name = "Admin" },
            new Role { Id = new Guid("50000000-0000-0000-0000-000000000002"), Name = "Manager" },
            new Role { Id = new Guid("50000000-0000-0000-0000-000000000003"), Name = "WarehouseOperator" },
            new Role { Id = new Guid("50000000-0000-0000-0000-000000000004"), Name = "SalesAgent" }
        );
    }
}