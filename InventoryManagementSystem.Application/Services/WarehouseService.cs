using System;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class WarehouseService : BaseService<Warehouse>, IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepo;
    private readonly IResourceRepository _resourceRepo;

    public WarehouseService(IWarehouseRepository repo, IResourceRepository resourceRepo) : base(repo)
    {
        _warehouseRepo = repo;
        _resourceRepo = resourceRepo;
    }

    public override async Task AddAsync(Warehouse warehouse)
    {
        var resource = new Resource
        {
            Id = Guid.NewGuid(),
            Type = "Warehouse",
            Name = warehouse.Name
        };
        await _resourceRepo.AddAsync(resource);
        warehouse.ResourceId = resource.Id;
        await _warehouseRepo.AddAsync(warehouse);
    }

    public async Task UpdateAsync(Warehouse warehouse)
    {
        await _warehouseRepo.UpdateAsync(warehouse);
        if (warehouse.ResourceId.HasValue)
        {
            var resource = await _resourceRepo.GetByIdAsync(warehouse.ResourceId.Value);
            if (resource != null && resource.Name != warehouse.Name)
            {
                resource.Name = warehouse.Name;
                await _resourceRepo.UpdateAsync(resource);
            }
        }
    }

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