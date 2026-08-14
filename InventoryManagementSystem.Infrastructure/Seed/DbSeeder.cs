using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Infrastructure.Seed;

public static class DbSeeder
{
    public static readonly Guid AdminUserId = new("40000000-0000-0000-0000-000000000001");
    public static readonly Guid AhmedOperatorUserId = new("40000000-0000-0000-0000-000000000002");
    public static readonly Guid MohamedOperatorUserId = new("40000000-0000-0000-0000-000000000003");
    public static readonly Guid ManagerUserId = new("40000000-0000-0000-0000-000000000004");

    private static readonly Guid MainWarehouseId = new("20000000-0000-0000-0000-000000000001");
    private static readonly Guid AlexandriaWarehouseId = new("20000000-0000-0000-0000-000000000002");

    private static readonly Guid Iphone15Id = new("30000000-0000-0000-0000-000000000001");
    private static readonly Guid SamsungTvId = new("30000000-0000-0000-0000-000000000002");
    private static readonly Guid DellXpsId = new("30000000-0000-0000-0000-000000000003");

    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedUsersAsync(context);
        await SeedOperatorAssignmentsAsync(context);
        await SeedTransactionsAsync(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var users = new[]
        {
            new User
            {
                Id = AdminUserId,
                Username = "admin",
                Email = "admin@inventory.local",
                DisplayName = "System Admin",
                Role = "Admin",
                PasswordHash = Authentication.PasswordHasher.Hash("Admin@123"),
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = AhmedOperatorUserId,
                Username = "ahmed",
                Email = "ahmed@inventory.local",
                DisplayName = "Ahmed Hassan",
                Role = "WarehouseOperator",
                PasswordHash = Authentication.PasswordHasher.Hash("Operator@123"),
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = MohamedOperatorUserId,
                Username = "mohamed",
                Email = "mohamed@inventory.local",
                DisplayName = "Mohamed Ali",
                Role = "WarehouseOperator",
                PasswordHash = Authentication.PasswordHasher.Hash("Operator@123"),
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = ManagerUserId,
                Username = "manager",
                Email = "manager@inventory.local",
                DisplayName = "Manager",
                Role = "Manager",
                PasswordHash = Authentication.PasswordHasher.Hash("Manager@123"),
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        context.Users.AddRange(users);
    }

    private static async Task SeedOperatorAssignmentsAsync(AppDbContext context)
    {
        if (await context.WarehouseOperators.AnyAsync())
        {
            return;
        }

        context.WarehouseOperators.AddRange(
            new WarehouseOperator { Id = Guid.NewGuid(), WarehouseId = MainWarehouseId, OperatorUserId = AhmedOperatorUserId },
            new WarehouseOperator { Id = Guid.NewGuid(), WarehouseId = AlexandriaWarehouseId, OperatorUserId = MohamedOperatorUserId }
        );
    }

    private static async Task SeedTransactionsAsync(AppDbContext context)
    {
        if (await context.InventoryTransactions.AnyAsync())
        {
            return;
        }

        context.InventoryTransactions.AddRange(
            new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = Iphone15Id,
                WarehouseId = MainWarehouseId,
                Type = TransactionType.Initial,
                QuantityChange = 50,
                PreviousQuantity = 0,
                NewQuantity = 50,
                Reason = "Initial stock on setup",
                CreatedByUserId = AdminUserId,
                CreatedAt = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },
            new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = Iphone15Id,
                WarehouseId = MainWarehouseId,
                Type = TransactionType.Increase,
                QuantityChange = 20,
                PreviousQuantity = 50,
                NewQuantity = 70,
                Reason = "New shipment received",
                CreatedByUserId = AdminUserId,
                CreatedAt = new DateTime(2026, 2, 1, 10, 30, 0, DateTimeKind.Utc)
            },
            new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = Iphone15Id,
                WarehouseId = MainWarehouseId,
                Type = TransactionType.Decrease,
                QuantityChange = 20,
                PreviousQuantity = 70,
                NewQuantity = 50,
                Reason = "Damaged products",
                CreatedByUserId = AhmedOperatorUserId,
                CreatedAt = new DateTime(2026, 2, 20, 14, 15, 0, DateTimeKind.Utc)
            },
            new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = SamsungTvId,
                WarehouseId = MainWarehouseId,
                Type = TransactionType.Initial,
                QuantityChange = 15,
                PreviousQuantity = 0,
                NewQuantity = 15,
                Reason = "Initial stock on setup",
                CreatedByUserId = AdminUserId,
                CreatedAt = new DateTime(2026, 1, 15, 8, 5, 0, DateTimeKind.Utc)
            },
            new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = DellXpsId,
                WarehouseId = AlexandriaWarehouseId,
                Type = TransactionType.Initial,
                QuantityChange = 8,
                PreviousQuantity = 0,
                NewQuantity = 8,
                Reason = "Initial stock on setup",
                CreatedByUserId = AdminUserId,
                CreatedAt = new DateTime(2026, 1, 20, 9, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}