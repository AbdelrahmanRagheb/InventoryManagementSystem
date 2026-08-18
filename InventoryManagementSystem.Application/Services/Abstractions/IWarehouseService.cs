using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IWarehouseService : IService<Warehouse>
{
    Task<PagedResponse<Warehouse>> GetPagedVisibleAsync(int page, int pageSize, Guid userId);
    Task UpdateAsync(Warehouse warehouse);
    Task<Warehouse?> ActivateAsync(Guid id);
}