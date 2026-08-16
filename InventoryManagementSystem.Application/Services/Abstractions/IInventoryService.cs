using InventoryManagementSystem.Application.DTOs.Inventory;
using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IInventoryService
{
    Task<Inventory?> GetByIdAsync(Guid id);
    Task<Inventory?> GetByProductWarehouseAsync(Guid productId, Guid warehouseId);
    Task<IReadOnlyList<Inventory>> GetAllAsync();
    Task<IReadOnlyList<Inventory>> GetByProductAsync(Guid productId);
    Task<IReadOnlyList<Inventory>> GetByWarehouseAsync(Guid warehouseId);
    Task<bool> ExistsAsync(Guid productId, Guid warehouseId);
    Task<(bool Success, string? Error)> CreateAsync(CreateInventoryRequest request, Guid createdByUserId);
    Task<(bool Success, string? Error)> AdjustAsync(AdjustStockRequest request, Guid changedByUserId);
}