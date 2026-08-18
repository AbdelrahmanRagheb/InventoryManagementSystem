using System.Collections.Generic;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdWithItemsAsync(Guid id);
    Task<IReadOnlyList<Order>> GetAllWithItemsAsync();
    Task<IReadOnlyList<Order>> GetByUserAsync(Guid userId);
    Task<(IReadOnlyList<Order> Items, int Total)> GetPagedWithItemsAsync(int page, int pageSize, IReadOnlyList<Guid>? assignedWarehouseIds);
    Task<(IReadOnlyList<Order> Items, int Total)> GetByUserPagedAsync(Guid userId, int page, int pageSize);
    Task UpdateAsync(Order entity);
}