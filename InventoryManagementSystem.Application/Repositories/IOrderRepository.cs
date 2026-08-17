using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdWithItemsAsync(Guid id);
    Task<IReadOnlyList<Order>> GetAllWithItemsAsync();
    Task<IReadOnlyList<Order>> GetByUserAsync(Guid userId);
    Task UpdateAsync(Order entity);
}