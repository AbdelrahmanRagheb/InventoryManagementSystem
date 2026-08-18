using System.Collections.Generic;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IInventoryTransactionRepository : IRepository<InventoryTransaction>
{
    Task<IReadOnlyList<InventoryTransaction>> GetByWarehouseAsync(Guid warehouseId);
    Task<IReadOnlyList<InventoryTransaction>> GetByProductAsync(Guid productId);
    Task<(IReadOnlyList<InventoryTransaction> Items, int Total)> GetPagedAsync(int page, int pageSize, IReadOnlyList<Guid>? warehouseIds);
    Task<(IReadOnlyList<InventoryTransaction> Items, int Total)> GetByProductPagedAsync(Guid productId, int page, int pageSize, IReadOnlyList<Guid>? warehouseIds);
    Task<(IReadOnlyList<InventoryTransaction> Items, int Total)> GetByWarehousePagedAsync(Guid warehouseId, int page, int pageSize);
}
