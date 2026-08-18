using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventoryManagementSystem.Infrastructure.Authentication;

namespace InventoryManagementSystem.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.DisplayName)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new User
            {
                Id = new Guid("40000000-0000-0000-0000-000000000001"),
                Username = "admin",
                Email = "admin@inventory.local",
                DisplayName = "System Admin",
                RoleId = new Guid("50000000-0000-0000-0000-000000000001"),
                PasswordHash = "100000.MBzYxHzXa5ldDEWJ/l9oQg==.MP5l6mZmU1DpWNWDFJJoughwJ83sLGWVEN1DC+t9I5M=",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = new Guid("40000000-0000-0000-0000-000000000002"),
                Username = "admin2",
                Email = "admin2@inventory.local",
                DisplayName = "Admin2",
                RoleId = new Guid("50000000-0000-0000-0000-000000000001"),
                PasswordHash = "100000.6u4ePiGDvOinOE45tjhHsA==.f4z3B7MINBuJu9tcM17xDV9J5rwCyUtaAcv93OBJtAA=",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = new Guid("40000000-0000-0000-0000-000000000003"),
                Username = "ahmed",
                Email = "ahmed@inventory.local",
                DisplayName = "Ahmed Hassan",
                RoleId = new Guid("50000000-0000-0000-0000-000000000003"),
                PasswordHash = "100000.Bs0wWo3rhMJ4dh2IGmp8BQ==.coVOKfh2h6uNEcjuP1uqkSxBlfZ1HV5/nSa03XCJhcM=",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = new Guid("40000000-0000-0000-0000-000000000004"),
                Username = "sara",
                Email = "sara@inventory.local",
                DisplayName = "Sara Mohamed",
                RoleId = new Guid("50000000-0000-0000-0000-000000000003"),
                PasswordHash = "100000.IUNFcOuRzyrW2TR497uubA==.6gQmot5WpeY3qPVa+71QKNJsAf8IUhWpOGn5Ri7YHiY=",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = new Guid("40000000-0000-0000-0000-000000000005"),
                Username = "mohamed",
                Email = "mohamed@inventory.local",
                DisplayName = "Mohamed Ali",
                RoleId = new Guid("50000000-0000-0000-0000-000000000003"),
                PasswordHash = "100000.j3jvp6L3MnN649wsRRMT6A==.X+FdMk0qGX1xq4tGgg3PcoQGeW3ddONzDvFCG0vC0o0=",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = new Guid("40000000-0000-0000-0000-000000000006"),
                Username = "manager",
                Email = "manager@inventory.local",
                DisplayName = "Manager",
                RoleId = new Guid("50000000-0000-0000-0000-000000000002"),
                PasswordHash = "100000.k19uFWjMCAzN+mGFdto86w==.z7SieNjkQ+HoYMlIfG815EZr7r54bCc/jF8FHWHc5Do=",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = new Guid("40000000-0000-0000-0000-000000000007"),
                Username = "sales1",
                Email = "sales1@inventory.local",
                DisplayName = "Sales Agent One",
                RoleId = new Guid("50000000-0000-0000-0000-000000000004"),
                PasswordHash = "100000.dxsBi574BJhxiOFR6stu7Q==.zY8XemzTza1R4uCnyBq2v+con1+G6srKZcCsxJfEiFg=",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}