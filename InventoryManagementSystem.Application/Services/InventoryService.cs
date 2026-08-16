using InventoryManagementSystem.Application.DTOs.Inventory;
using InventoryManagementSystem.Application.DTOs.Users;
using InventoryManagementSystem.Domain.Enums;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class InventoryService
{
    private readonly IInventoryRepository _invRepo;
    private readonly IInventoryTransactionRepository _txnRepo;
    private readonly IWarehouseOperatorRepository _opRepo;

    public InventoryService(IInventoryRepository invRepo, IInventoryTransactionRepository txnRepo, IWarehouseOperatorRepository opRepo)
    {
        _invRepo = invRepo;
        _txnRepo = txnRepo;
        _opRepo = opRepo;
    }

    public async Task<Inventory?> GetByIdAsync(Guid id) => await _invRepo.GetByIdAsync(id);
    public async Task<Inventory?> GetByProductWarehouseAsync(Guid productId, Guid warehouseId) =>
        await _invRepo.GetByProductWarehouseAsync(productId, warehouseId);
    public async Task<IReadOnlyList<Inventory>> GetAllAsync() => await _invRepo.GetAllAsync();
    public async Task<IReadOnlyList<Inventory>> GetByProductAsync(Guid productId) => await _invRepo.GetByProductAsync(productId);
    public async Task<IReadOnlyList<Inventory>> GetByWarehouseAsync(Guid warehouseId) => await _invRepo.GetByWarehouseAsync(warehouseId);

    public async Task<bool> ExistsAsync(Guid productId, Guid warehouseId) => await _invRepo.GetByProductWarehouseAsync(productId, warehouseId) != null;

    private async Task<(bool IsOperator, Guid OwnedWarehouseId)> CheckOperatorWarehouse(Guid userId)
    {
        var op = await _opRepo.GetByOperatorAsync(userId);
        return op.Count > 0 ? (true, op[0].WarehouseId) : (false, Guid.Empty);
    }

    public async Task<(bool Success, string? Error)> CreateAsync(CreateInventoryRequest request, Guid createdByUserId)
    {
        if (await ExistsAsync(request.ProductId, request.WarehouseId))
            return (false, "Inventory record already exists for this product and warehouse");

        var inventory = new Inventory
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            WarehouseId = request.WarehouseId,
            Quantity = request.Quantity,
            UpdatedAt = DateTime.UtcNow
        };

        var transaction = new InventoryTransaction
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            WarehouseId = request.WarehouseId,
            Type = TransactionType.Initial,
            QuantityChange = request.Quantity,
            PreviousQuantity = 0,
            NewQuantity = request.Quantity,
            Reason = request.Reason,
            CreatedByUserId = createdByUserId,
            CreatedAt = DateTime.UtcNow
        };

        await _invRepo.AddAsync(inventory);
        await _txnRepo.AddAsync(transaction);
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> AdjustAsync(AdjustStockRequest request, Guid changedByUserId)
    {
        var inventory = await _invRepo.GetByProductWarehouseAsync(request.ProductId, request.WarehouseId);
        if (inventory == null) return (false, "Inventory record not found");

        if (request.QuantityChange <= 0) return (false, "Quantity change must be positive");

        if (string.IsNullOrWhiteSpace(request.Reason)) return (false, "Reason is required");

        var (isOperator, ownedWh) = await CheckOperatorWarehouse(changedByUserId);
        if (isOperator && inventory.WarehouseId != ownedWh)
            return (false, "Warehouse operator can only adjust their assigned warehouse");

        if (inventory.Quantity - request.QuantityChange < 0) return (false, "Decrease would result in negative stock");

        inventory.Quantity -= request.QuantityChange;
        inventory.UpdatedAt = DateTime.UtcNow;
        await _invRepo.UpdateAsync(inventory);

        var txnType = request.QuantityChange > 0 ? TransactionType.Increase : TransactionType.Decrease;
        var transaction = new InventoryTransaction
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            WarehouseId = request.WarehouseId,
            Type = txnType,
            QuantityChange = Math.Abs(request.QuantityChange),
            PreviousQuantity = inventory.Quantity + request.QuantityChange,
            NewQuantity = inventory.Quantity,
            Reason = request.Reason,
            CreatedByUserId = changedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        await _txnRepo.AddAsync(transaction);
        return (true, null);
    }

    }