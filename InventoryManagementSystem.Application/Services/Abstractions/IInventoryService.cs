using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Application.DTOs.Inventory;
using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IInventoryService
{
    Task<(Inventory? Inventory, bool Forbidden)> GetByIdAsync(Guid id, Guid userId);
    Task<Inventory?> GetByProductWarehouseAsync(Guid productId, Guid warehouseId);
    Task<PagedResponse<Inventory>> GetAllPagedAsync(int page, int pageSize, Guid userId);
    Task<PagedResponse<Inventory>> GetByProductPagedAsync(Guid productId, int page, int pageSize, Guid userId);
    Task<(PagedResponse<Inventory>? Page, bool Forbidden)> GetByWarehousePagedAsync(Guid warehouseId, int page, int pageSize, Guid userId);
    Task<bool> ExistsAsync(Guid productId, Guid warehouseId);
    Task<(bool Success, string? Error, bool Forbidden)> CreateAsync(CreateInventoryRequest request, Guid createdByUserId);
    Task<(bool Success, string? Error, bool Forbidden)> AdjustAsync(AdjustStockRequest request, Guid changedByUserId);
}