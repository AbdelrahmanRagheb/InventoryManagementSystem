using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IWarehouseRepository : IRepository<Warehouse>
{
    Task UpdateAsync(Warehouse entity);
}
