using System;
using System.Collections.Generic;
using System.Linq;
using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class WarehouseService : BaseService<Warehouse>, IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepo;
    private readonly IResourceRepository _resourceRepo;
    private readonly IAccessService _accessService;
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(
        IWarehouseRepository repo,
        IResourceRepository resourceRepo,
        IAccessService accessService,
        IUnitOfWork unitOfWork) : base(repo)
    {
        _warehouseRepo = repo;
        _resourceRepo = resourceRepo;
        _accessService = accessService;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponse<Warehouse>> GetPagedVisibleAsync(int page, int pageSize, Guid userId)
    {
        var (p, ps) = Paging.Normalize(page, pageSize);
        IReadOnlyList<Guid>? warehouseIds = null;
        if (await _accessService.IsRestrictedToAssignedWarehousesAsync(userId))
            warehouseIds = await _accessService.GetAssignedWarehouseIdsAsync(userId);
        var (items, total) = await _warehouseRepo.GetPagedAsync(p, ps, warehouseIds);
        return new PagedResponse<Warehouse>(items, p, ps, total);
    }

    public override async Task AddAsync(Warehouse warehouse)
    {
        var resource = new Resource
        {
            Id = Guid.NewGuid(),
            Type = "Warehouse",
            Name = warehouse.Name
        };
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _resourceRepo.AddAsync(resource);
            warehouse.ResourceId = resource.Id;
            await _warehouseRepo.AddAsync(warehouse);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(Warehouse warehouse)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
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
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
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