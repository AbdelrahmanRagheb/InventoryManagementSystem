using System.Collections.Generic;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IInventoryRepository : IRepository<Inventory>
{
    Task<Inventory?> GetByProductWarehouseAsync(Guid productId, Guid warehouseId);
    Task<IReadOnlyList<Inventory>> GetByProductAsync(Guid productId);
    Task<IReadOnlyList<Inventory>> GetByWarehouseAsync(Guid warehouseId);
    Task<(IReadOnlyList<Inventory> Items, int Total)> GetPagedAsync(int page, int pageSize, IReadOnlyList<Guid>? warehouseIds);
    Task<(IReadOnlyList<Inventory> Items, int Total)> GetByProductPagedAsync(Guid productId, int page, int pageSize, IReadOnlyList<Guid>? warehouseIds);
    Task<(IReadOnlyList<Inventory> Items, int Total)> GetByWarehousePagedAsync(Guid warehouseId, int page, int pageSize);
    Task RemoveAsync(Guid id);
    Task UpdateAsync(Inventory entity);
}
