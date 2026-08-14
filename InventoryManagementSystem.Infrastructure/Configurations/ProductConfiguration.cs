using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(p => p.IsActive);

        builder.HasData(
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000001"), Name = "iPhone 15", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000002"), Name = "Samsung 55\" Smart TV", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000003"), Name = "Dell XPS 13 Laptop", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000004"), Name = "Ergonomic Office Chair", CategoryId = new Guid("10000000-0000-0000-0000-000000000003"), IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000005"), Name = "Basmati Rice 5kg", CategoryId = new Guid("10000000-0000-0000-0000-000000000002"), IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}