using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IInventoryRepository : IRepository<Inventory>
{
    Task<Inventory?> GetByProductWarehouseAsync(Guid productId, Guid warehouseId);
    Task<IReadOnlyList<Inventory>> GetByProductAsync(Guid productId);
    Task<IReadOnlyList<Inventory>> GetByWarehouseAsync(Guid warehouseId);
    Task RemoveAsync(Guid id);
    Task UpdateAsync(Inventory entity);
}
