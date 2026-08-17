using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class WarehouseService : BaseService<Warehouse>, IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepo;
    public WarehouseService(IWarehouseRepository repo) : base(repo) => _warehouseRepo = repo;

    public async Task UpdateAsync(Warehouse warehouse) { await _warehouseRepo.UpdateAsync(warehouse); }

    public override async Task DeactivateAsync(Guid id)
    {
        var wh = await _warehouseRepo.GetByIdAsync(id);
        if (wh != null) { wh.IsActive = false; await _warehouseRepo.UpdateAsync(wh); }
    }

    public async Task<Warehouse?> ActivateAsync(Guid id)
    {
        var wh = await _warehouseRepo.GetByIdAsync(id);
        if (wh == null) return null;
        wh.IsActive = true;
        await _warehouseRepo.UpdateAsync(wh);
        return wh;
    }
}