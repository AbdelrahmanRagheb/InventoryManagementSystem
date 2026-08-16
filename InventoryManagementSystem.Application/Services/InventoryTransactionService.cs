using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class InventoryTransactionService
{
    private readonly IInventoryTransactionRepository _repo;
    public InventoryTransactionService(IInventoryTransactionRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<InventoryTransaction>> GetByWarehouseAsync(Guid warehouseId) => await _repo.GetByWarehouseAsync(warehouseId);
    public async Task<IReadOnlyList<InventoryTransaction>> GetByProductAsync(Guid productId) => await _repo.GetByProductAsync(productId);
    public async Task<IReadOnlyList<InventoryTransaction>> GetAllAsync() => await _repo.GetAllAsync();
}
