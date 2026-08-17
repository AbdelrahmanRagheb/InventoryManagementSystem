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

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000001"), Name = "iPhone 15", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), Price = 799.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000002"), Name = "Samsung 55 TV", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), Price = 649.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000003"), Name = "Dell XPS 13 Laptop", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), Price = 999.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000004"), Name = "Bluetooth Headphones", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), Price = 89.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000005"), Name = "Smart Watch", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), Price = 199.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000006"), Name = "Basmati Rice 5kg", CategoryId = new Guid("10000000-0000-0000-0000-000000000002"), Price = 12.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000007"), Name = "Canned Beans", CategoryId = new Guid("10000000-0000-0000-0000-000000000002"), Price = 2.49m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000008"), Name = "Olive Oil", CategoryId = new Guid("10000000-0000-0000-0000-000000000002"), Price = 9.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000009"), Name = "Fresh Bread", CategoryId = new Guid("10000000-0000-0000-0000-000000000002"), Price = 1.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000010"), Name = "Almonds", CategoryId = new Guid("10000000-0000-0000-0000-000000000002"), Price = 8.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000011"), Name = "Office Chair", CategoryId = new Guid("10000000-0000-0000-0000-000000000003"), Price = 149.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000012"), Name = "Desk Lamp", CategoryId = new Guid("10000000-0000-0000-0000-000000000003"), Price = 39.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000013"), Name = "Notebook Set", CategoryId = new Guid("10000000-0000-0000-0000-000000000003"), Price = 5.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000014"), Name = "Pen Set", CategoryId = new Guid("10000000-0000-0000-0000-000000000003"), Price = 3.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000015"), Name = "Stapler", CategoryId = new Guid("10000000-0000-0000-0000-000000000003"), Price = 7.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000016"), Name = "T-Shirt", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), Price = 15.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000017"), Name = "Jeans", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), Price = 49.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000018"), Name = "Jacket", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), Price = 89.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000019"), Name = "Coffee Maker", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), Price = 79.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = new Guid("30000000-0000-0000-0000-000000000020"), Name = "Blender", CategoryId = new Guid("10000000-0000-0000-0000-000000000001"), Price = 59.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}