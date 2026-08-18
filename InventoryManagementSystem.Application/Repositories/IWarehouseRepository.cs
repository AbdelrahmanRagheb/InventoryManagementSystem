using InventoryManagementSystem.Application.Repositories;
using System.Collections.Generic;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IWarehouseRepository : IRepository<Warehouse>
{
    Task UpdateAsync(Warehouse entity);
    Task<(IReadOnlyList<Warehouse> Items, int Total)> GetPagedAsync(int page, int pageSize, IReadOnlyList<Guid>? warehouseIds);
}
