using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Services;

public class InventoryTransactionService : IInventoryTransactionService
{
    private readonly IInventoryTransactionRepository _repo;
    private readonly IAccessService _accessService;

    public InventoryTransactionService(IInventoryTransactionRepository repo, IAccessService accessService)
    {
        _repo = repo;
        _accessService = accessService;
    }

    public async Task<PagedResponse<InventoryTransaction>> GetAllPagedAsync(int page, int pageSize, Guid userId)
    {
        var (p, ps) = Paging.Normalize(page, pageSize);
        var warehouseIds = await GetVisibilityWarehouseIdsAsync(userId);
        var (items, total) = await _repo.GetPagedAsync(p, ps, warehouseIds);
        return new PagedResponse<InventoryTransaction>(items, p, ps, total);
    }

    public async Task<PagedResponse<InventoryTransaction>> GetByProductPagedAsync(Guid productId, int page, int pageSize, Guid userId)
    {
        var (p, ps) = Paging.Normalize(page, pageSize);
        var warehouseIds = await GetVisibilityWarehouseIdsAsync(userId);
        var (items, total) = await _repo.GetByProductPagedAsync(productId, p, ps, warehouseIds);
        return new PagedResponse<InventoryTransaction>(items, p, ps, total);
    }

    public async Task<(PagedResponse<InventoryTransaction>? Page, bool Forbidden)> GetByWarehousePagedAsync(Guid warehouseId, int page, int pageSize, Guid userId)
    {
        if (await _accessService.IsRestrictedToAssignedWarehousesAsync(userId))
        {
            var assigned = await _accessService.GetAssignedWarehouseIdsAsync(userId);
            if (!assigned.Contains(warehouseId)) return (null, true);
        }
        var (p, ps) = Paging.Normalize(page, pageSize);
        var (items, total) = await _repo.GetByWarehousePagedAsync(warehouseId, p, ps);
        return (new PagedResponse<InventoryTransaction>(items, p, ps, total), false);
    }

    private async Task<IReadOnlyList<Guid>?> GetVisibilityWarehouseIdsAsync(Guid userId)
    {
        if (!await _accessService.IsRestrictedToAssignedWarehousesAsync(userId))
            return null;
        return await _accessService.GetAssignedWarehouseIdsAsync(userId);
    }
}
