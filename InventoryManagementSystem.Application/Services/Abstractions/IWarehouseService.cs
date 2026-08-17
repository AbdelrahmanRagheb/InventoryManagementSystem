using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IWarehouseService : IService<Warehouse>
{
    Task UpdateAsync(Warehouse warehouse);
    Task<Warehouse?> ActivateAsync(Guid id);
}