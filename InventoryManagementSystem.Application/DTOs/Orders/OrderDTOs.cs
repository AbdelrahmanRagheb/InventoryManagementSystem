using InventoryManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Application.DTOs.Orders;

public record OrderItemRequest(
    [param: Required] Guid ProductId,
    [param: Range(1, int.MaxValue)] int Quantity);

public record CreateOrderRequest(
    [param: Required, StringLength(150)] string CustomerName,
    [param: EmailAddress, StringLength(150)] string? CustomerEmail,
    [param: Required] List<OrderItemRequest> Items);

public record UpdateOrderRequest(
    [param: StringLength(150)] string? CustomerName = null,
    [param: EmailAddress, StringLength(150)] string? CustomerEmail = null,
    List<OrderItemRequest>? Items = null);

public record FulfillOrderRequest([param: Required] Guid WarehouseId);

public record OrderItemResponse(Guid Id, Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);

public record OrderHistoryResponse(Guid Id, OrderStatus Status, Guid ChangedByUserId, DateTime ChangedAt);

public record OrderResponse(
    Guid Id,
    string CustomerName,
    string? CustomerEmail,
    OrderStatus Status,
    Guid CreatedByUserId,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    DateTime? CancelledAt,
    decimal Total,
    List<OrderItemResponse> Items,
    List<OrderHistoryResponse> History);