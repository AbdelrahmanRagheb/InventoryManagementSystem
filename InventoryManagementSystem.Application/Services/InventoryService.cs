using InventoryManagementSystem.Application.Authorization;
using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Application.DTOs.Inventory;
using InventoryManagementSystem.Application.DTOs.Users;
using InventoryManagementSystem.Domain.Enums;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _invRepo;
    private readonly IInventoryTransactionRepository _txnRepo;
    private readonly IAccessService _accessService;
    private readonly IWarehouseRepository _warehouseRepo;
    private readonly IUnitOfWork _unitOfWork;

    public InventoryService(
        IInventoryRepository invRepo,
        IInventoryTransactionRepository txnRepo,
        IAccessService accessService,
        IWarehouseRepository warehouseRepo,
        IUnitOfWork unitOfWork)
    {
        _invRepo = invRepo;
        _txnRepo = txnRepo;
        _accessService = accessService;
        _warehouseRepo = warehouseRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<(Inventory? Inventory, bool Forbidden)> GetByIdAsync(Guid id, Guid userId)
    {
        var inventory = await _invRepo.GetByIdAsync(id);
        if (inventory == null) return (null, false);
        if (await _accessService.IsRestrictedToAssignedWarehousesAsync(userId))
        {
            var assigned = await _accessService.GetAssignedWarehouseIdsAsync(userId);
            if (!assigned.Contains(inventory.WarehouseId)) return (null, true);
        }
        return (inventory, false);
    }

    public async Task<Inventory?> GetByProductWarehouseAsync(Guid productId, Guid warehouseId) =>
        await _invRepo.GetByProductWarehouseAsync(productId, warehouseId);

    public async Task<PagedResponse<Inventory>> GetAllPagedAsync(int page, int pageSize, Guid userId)
    {
        var (p, ps) = Paging.Normalize(page, pageSize);
        var warehouseIds = await GetVisibilityWarehouseIdsAsync(userId);
        var (items, total) = await _invRepo.GetPagedAsync(p, ps, warehouseIds);
        return new PagedResponse<Inventory>(items, p, ps, total);
    }

    public async Task<PagedResponse<Inventory>> GetByProductPagedAsync(Guid productId, int page, int pageSize, Guid userId)
    {
        var (p, ps) = Paging.Normalize(page, pageSize);
        var warehouseIds = await GetVisibilityWarehouseIdsAsync(userId);
        var (items, total) = await _invRepo.GetByProductPagedAsync(productId, p, ps, warehouseIds);
        return new PagedResponse<Inventory>(items, p, ps, total);
    }

    public async Task<(PagedResponse<Inventory>? Page, bool Forbidden)> GetByWarehousePagedAsync(Guid warehouseId, int page, int pageSize, Guid userId)
    {
        if (await _accessService.IsRestrictedToAssignedWarehousesAsync(userId))
        {
            var assigned = await _accessService.GetAssignedWarehouseIdsAsync(userId);
            if (!assigned.Contains(warehouseId)) return (null, true);
        }
        var (p, ps) = Paging.Normalize(page, pageSize);
        var (items, total) = await _invRepo.GetByWarehousePagedAsync(warehouseId, p, ps);
        return (new PagedResponse<Inventory>(items, p, ps, total), false);
    }

    private async Task<IReadOnlyList<Guid>?> GetVisibilityWarehouseIdsAsync(Guid userId)
    {
        if (!await _accessService.IsRestrictedToAssignedWarehousesAsync(userId))
            return null;
        return await _accessService.GetAssignedWarehouseIdsAsync(userId);
    }

    public async Task<bool> ExistsAsync(Guid productId, Guid warehouseId) => await _invRepo.GetByProductWarehouseAsync(productId, warehouseId) != null;

    public async Task<(bool Success, string? Error, bool Forbidden)> CreateAsync(CreateInventoryRequest request, Guid createdByUserId)
    {
        if (!await _accessService.CanAsync(createdByUserId, PermissionCatalog.InventoryAdjust, request.WarehouseId))
            return (false, "You are not allowed to adjust stock in this warehouse", true);

        var warehouse = await _warehouseRepo.GetByIdAsync(request.WarehouseId);
        if (warehouse == null) return (false, "Warehouse not found", false);
        if (!warehouse.IsActive) return (false, "Warehouse is deactivated", false);

        if (await ExistsAsync(request.ProductId, request.WarehouseId))
            return (false, "Inventory record already exists for this product and warehouse", false);

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

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _invRepo.AddAsync(inventory);
            await _txnRepo.AddAsync(transaction);
            await _unitOfWork.CommitAsync();
            return (true, null, false);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<(bool Success, string? Error, bool Forbidden)> AdjustAsync(AdjustStockRequest request, Guid changedByUserId)
    {
        if (!await _accessService.CanAsync(changedByUserId, PermissionCatalog.InventoryAdjust, request.WarehouseId))
            return (false, "You are not allowed to adjust stock in this warehouse", true);

        var warehouse = await _warehouseRepo.GetByIdAsync(request.WarehouseId);
        if (warehouse == null) return (false, "Warehouse not found", false);
        if (!warehouse.IsActive) return (false, "Warehouse is deactivated", false);

        var inventory = await _invRepo.GetByProductWarehouseAsync(request.ProductId, request.WarehouseId);
        if (inventory == null) return (false, "Inventory record not found", false);

        if (request.QuantityChange <= 0) return (false, "Quantity change must be positive", false);

        if (request.Type != TransactionType.Increase && request.Type != TransactionType.Decrease)
            return (false, "Transaction type must be Increase or Decrease", false);

        if (string.IsNullOrWhiteSpace(request.Reason)) return (false, "Reason is required", false);

        var newQuantity = request.Type == TransactionType.Increase
            ? inventory.Quantity + request.QuantityChange
            : inventory.Quantity - request.QuantityChange;

        if (newQuantity < 0) return (false, "Decrease would result in negative stock", false);

        var previousQuantity = inventory.Quantity;
        inventory.Quantity = newQuantity;
        inventory.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _invRepo.UpdateAsync(inventory);

            var transaction = new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                WarehouseId = request.WarehouseId,
                Type = request.Type,
                QuantityChange = request.QuantityChange,
                PreviousQuantity = previousQuantity,
                NewQuantity = newQuantity,
                Reason = request.Reason,
                CreatedByUserId = changedByUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _txnRepo.AddAsync(transaction);
            await _unitOfWork.CommitAsync();
            return (true, null, false);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    }