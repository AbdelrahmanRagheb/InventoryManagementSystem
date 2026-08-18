using InventoryManagementSystem.Application.Authorization;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace InventoryManagementSystem.Infrastructure.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(300);

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.HasData(
            PermissionCatalog.Ids
                .Select(kvp => new Permission
                {
                    Id = kvp.Value,
                    Name = kvp.Key,
                    Description = PermissionCatalog.Descriptions[kvp.Key]
                })
                .ToList());
    }
}