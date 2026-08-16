using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class WarehouseService : BaseService<Warehouse>
{
    private readonly IWarehouseRepository _warehouseRepo;
    public WarehouseService(IWarehouseRepository repo) : base(repo) => _warehouseRepo = repo;

    public async Task UpdateAsync(Warehouse warehouse) { await _warehouseRepo.UpdateAsync(warehouse); }

    public override async Task DeleteAsync(Guid id)
    {
        var wh = await _warehouseRepo.GetByIdAsync(id);
        if (wh != null) { wh.IsActive = false; await _warehouseRepo.UpdateAsync(wh); }
    }
}