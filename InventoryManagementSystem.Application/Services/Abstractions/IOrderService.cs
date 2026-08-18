using InventoryManagementSystem.Application.DTOs.Orders;
using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IOrderService
{
    Task<IReadOnlyList<Order>> GetAllAsync();
    Task<IReadOnlyList<Order>> GetByUserAsync(Guid userId);
    Task<Order?> GetByIdWithItemsAsync(Guid id);
    Task<(bool Success, Order? Order, string? Error)> CreateAsync(CreateOrderRequest request, Guid createdByUserId);
    Task<(bool Success, string? Error, bool Forbidden)> UpdateAsync(Guid orderId, UpdateOrderRequest request, Guid userId);
    Task<(bool Success, string? Error, bool Forbidden)> CancelAsync(Guid orderId, Guid userId);
    Task<(bool Success, string? Error, bool Forbidden)> FulfillAsync(Guid orderId, FulfillOrderRequest request, Guid operatorUserId);
}