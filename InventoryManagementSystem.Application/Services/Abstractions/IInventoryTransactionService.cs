using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IInventoryTransactionService
{
    Task<IReadOnlyList<InventoryTransaction>> GetByWarehouseAsync(Guid warehouseId);
    Task<IReadOnlyList<InventoryTransaction>> GetByProductAsync(Guid productId);
    Task<IReadOnlyList<InventoryTransaction>> GetAllAsync();
}