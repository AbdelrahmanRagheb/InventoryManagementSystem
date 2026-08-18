using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IInventoryTransactionService
{
    Task<PagedResponse<InventoryTransaction>> GetAllPagedAsync(int page, int pageSize, Guid userId);
    Task<PagedResponse<InventoryTransaction>> GetByProductPagedAsync(Guid productId, int page, int pageSize, Guid userId);
    Task<(PagedResponse<InventoryTransaction>? Page, bool Forbidden)> GetByWarehousePagedAsync(Guid warehouseId, int page, int pageSize, Guid userId);
}