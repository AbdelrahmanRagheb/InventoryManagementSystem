using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IOperatorService
{
    Task<IReadOnlyList<WarehouseOperator>> GetByWarehouseAsync(Guid warehouseId);
    Task<IReadOnlyList<WarehouseOperator>> GetByOperatorAsync(Guid userId);
    Task<WarehouseOperator?> GetByWarehouseAndUserAsync(Guid warehouseId, Guid userId);
    Task AddAsync(WarehouseOperator assign);
    Task UpdateAsync(WarehouseOperator assign);
    Task RemoveAsync(Guid warehouseId, Guid userId);
}