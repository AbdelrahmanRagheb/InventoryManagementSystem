using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.HasQueryFilter(c => c.IsActive);

        builder.HasData(
            new Category { Id = new Guid("10000000-0000-0000-0000-000000000001"), Name = "Electronics", Description = "Electronic devices and accessories", IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = new Guid("10000000-0000-0000-0000-000000000002"), Name = "Groceries", Description = "Food and household goods", IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = new Guid("10000000-0000-0000-0000-000000000003"), Name = "Office Supplies", Description = "Office furniture and supplies", IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}