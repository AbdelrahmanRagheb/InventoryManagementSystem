using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class OperatorService : IOperatorService
{
    private readonly IWarehouseOperatorRepository _repo;
    public OperatorService(IWarehouseOperatorRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<WarehouseOperator>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<IReadOnlyList<WarehouseOperator>> GetByWarehouseAsync(Guid warehouseId) => await _repo.GetByWarehouseAsync(warehouseId);
    public async Task<IReadOnlyList<WarehouseOperator>> GetByOperatorAsync(Guid userId) => await _repo.GetByOperatorAsync(userId);
    public async Task<WarehouseOperator?> GetByWarehouseAndUserAsync(Guid warehouseId, Guid userId) => await _repo.GetByWarehouseAndUserAsync(warehouseId, userId);

    public async Task AddAsync(WarehouseOperator assign)
    {
        var exists = await _repo.GetByWarehouseAndUserAsync(assign.WarehouseId, assign.OperatorUserId);
        if (exists != null) throw new InvalidOperationException("Operator already assigned to this warehouse");
        await _repo.AddAsync(assign);
    }

    public async Task UpdateAsync(WarehouseOperator assign)
    {
        var existing = await _repo.GetByIdAsync(assign.Id);
        if (existing == null) throw new InvalidOperationException("Assignment not found");
        var duplicate = await _repo.GetByWarehouseAndUserAsync(assign.WarehouseId, assign.OperatorUserId);
        if (duplicate != null && duplicate.Id != assign.Id)
            throw new InvalidOperationException("Operator already assigned to this warehouse");
        existing.WarehouseId = assign.WarehouseId;
        existing.OperatorUserId = assign.OperatorUserId;
        await _repo.UpdateAsync(existing);
    }

    public async Task RemoveAsync(Guid warehouseId, Guid userId) => await _repo.RemoveAsync(warehouseId, userId);
}