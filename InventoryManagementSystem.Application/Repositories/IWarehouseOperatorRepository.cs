using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IWarehouseOperatorRepository : IRepository<WarehouseOperator>
{
    Task UpdateAsync(WarehouseOperator entity);
    Task<WarehouseOperator?> GetByWarehouseAndUserAsync(Guid warehouseId, Guid userId);
    Task<IReadOnlyList<WarehouseOperator>> GetByWarehouseAsync(Guid warehouseId);
    Task<IReadOnlyList<WarehouseOperator>> GetByOperatorAsync(Guid userId);
    Task RemoveAsync(Guid warehouseId, Guid userId);
}
