using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IInventoryTransactionRepository : IRepository<InventoryTransaction>
{
    Task<IReadOnlyList<InventoryTransaction>> GetByWarehouseAsync(Guid warehouseId);
    Task<IReadOnlyList<InventoryTransaction>> GetByProductAsync(Guid productId);
}
