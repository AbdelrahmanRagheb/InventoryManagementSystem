using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Application.DTOs.Orders;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IOrderService
{
    Task<PagedResponse<Order>> GetAllPagedAsync(int page, int pageSize, Guid userId);
    Task<PagedResponse<Order>> GetByUserPagedAsync(int page, int pageSize, Guid userId);
    Task<(Order? Order, bool Forbidden)> GetByIdWithItemsAsync(Guid id, Guid userId);
    Task<(bool Success, Order? Order, string? Error)> CreateAsync(CreateOrderRequest request, Guid createdByUserId);
    Task<(bool Success, string? Error, bool Forbidden)> UpdateAsync(Guid orderId, UpdateOrderRequest request, Guid userId);
    Task<(bool Success, string? Error, bool Forbidden)> CancelAsync(Guid orderId, Guid userId);
    Task<(bool Success, string? Error, bool Forbidden)> FulfillAsync(Guid orderId, FulfillOrderRequest request, Guid operatorUserId);
}