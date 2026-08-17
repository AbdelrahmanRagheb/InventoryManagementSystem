using InventoryManagementSystem.Application.DTOs.Orders;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IInventoryTransactionRepository _transactionRepo;
    private readonly IWarehouseOperatorRepository _warehouseOperatorRepo;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        IOrderRepository orderRepo,
        IProductRepository productRepo,
        IInventoryRepository inventoryRepo,
        IInventoryTransactionRepository transactionRepo,
        IWarehouseOperatorRepository warehouseOperatorRepo,
        IUnitOfWork unitOfWork)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
        _inventoryRepo = inventoryRepo;
        _transactionRepo = transactionRepo;
        _warehouseOperatorRepo = warehouseOperatorRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync() =>
        await _orderRepo.GetAllWithItemsAsync();

    public async Task<IReadOnlyList<Order>> GetByUserAsync(Guid userId) =>
        await _orderRepo.GetByUserAsync(userId);

    public async Task<Order?> GetByIdWithItemsAsync(Guid id) =>
        await _orderRepo.GetByIdWithItemsAsync(id);

    public async Task<(bool Success, Order? Order, string? Error)> CreateAsync(CreateOrderRequest request, Guid createdByUserId)
    {
        if (request.Items == null || request.Items.Count == 0)
            return (false, null, "Order must contain at least one item");

        var products = await _productRepo.GetByIdsAsync(request.Items.Select(i => i.ProductId).Distinct());
        var (items, error) = BuildItems(request.Items, products);
        if (error != null)
            return (false, null, error);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            Status = OrderStatus.Pending,
            CreatedByUserId = createdByUserId,
            CreatedAt = DateTime.UtcNow,
            Items = items!,
            History = new List<OrderHistory>
            {
                new OrderHistory
                {
                    Id = Guid.NewGuid(),
                    Status = OrderStatus.Pending,
                    ChangedByUserId = createdByUserId,
                    ChangedAt = DateTime.UtcNow
                }
            }
        };

        await _orderRepo.AddAsync(order);
        return (true, order, null);
    }

    public async Task<(bool Success, string? Error, bool Forbidden)> UpdateAsync(Guid orderId, UpdateOrderRequest request, Guid userId)
    {
        var order = await _orderRepo.GetByIdWithItemsAsync(orderId);
        if (order == null)
            return (false, $"Order with id: {orderId} does not exist", false);
        if (order.CreatedByUserId != userId)
            return (false, null, true);
        if (order.Status != OrderStatus.Pending)
            return (false, "Only pending orders can be updated", false);

        if (request.CustomerName != null) order.CustomerName = request.CustomerName;
        if (request.CustomerEmail != null) order.CustomerEmail = request.CustomerEmail;

        if (request.Items != null && request.Items.Count > 0)
        {
            var products = await _productRepo.GetByIdsAsync(request.Items.Select(i => i.ProductId).Distinct());
            var (items, error) = BuildItems(request.Items, products);
            if (error != null)
                return (false, error, false);

            order.Items.Clear();
            foreach (var item in items!)
                order.Items.Add(item);
        }

        await _orderRepo.UpdateAsync(order);
        return (true, null, false);
    }

    public async Task<(bool Success, string? Error, bool Forbidden)> CancelAsync(Guid orderId, Guid userId)
    {
        var order = await _orderRepo.GetByIdWithItemsAsync(orderId);
        if (order == null)
            return (false, $"Order with id: {orderId} does not exist", false);
        if (order.CreatedByUserId != userId)
            return (false, null, true);
        if (order.Status != OrderStatus.Pending)
            return (false, "Only pending orders can be cancelled", false);

        order.Status = OrderStatus.Cancelled;
        order.CancelledAt = DateTime.UtcNow;
        order.History.Add(new OrderHistory
        {
            Id = Guid.NewGuid(),
            Status = OrderStatus.Cancelled,
            ChangedByUserId = userId,
            ChangedAt = DateTime.UtcNow
        });
        await _orderRepo.UpdateAsync(order);
        return (true, null, false);
    }

    public async Task<(bool Success, string? Error, bool Forbidden)> FulfillAsync(Guid orderId, Guid warehouseId, Guid operatorUserId)
    {
        var order = await _orderRepo.GetByIdWithItemsAsync(orderId);
        if (order == null)
            return (false, $"Order with id: {orderId} does not exist", false);
        if (order.Status != OrderStatus.Pending)
            return (false, "Only pending orders can be fulfilled", false);

        var assignments = await _warehouseOperatorRepo.GetByOperatorAsync(operatorUserId);
        if (!assignments.Any(a => a.WarehouseId == warehouseId))
            return (false, "Warehouse operator can only fulfill orders from their assigned warehouse", true);

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            foreach (var item in order.Items)
            {
                var inventory = await _inventoryRepo.GetByProductWarehouseAsync(item.ProductId, warehouseId);
                if (inventory == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, $"No inventory record for product: {item.ProductId} in this warehouse", false);
                }
                if (inventory.Quantity < item.Quantity)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, $"Insufficient stock for product: {item.ProductId} in this warehouse", false);
                }

                var previous = inventory.Quantity;
                inventory.Quantity = previous - item.Quantity;
                inventory.UpdatedAt = DateTime.UtcNow;
                await _inventoryRepo.UpdateAsync(inventory);

                await _transactionRepo.AddAsync(new InventoryTransaction
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    WarehouseId = warehouseId,
                    Type = TransactionType.Decrease,
                    QuantityChange = item.Quantity,
                    PreviousQuantity = previous,
                    NewQuantity = inventory.Quantity,
                    Reason = $"Order fulfillment: {orderId}",
                    CreatedByUserId = operatorUserId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            order.Status = OrderStatus.Completed;
            order.CompletedAt = DateTime.UtcNow;
            order.History.Add(new OrderHistory
            {
                Id = Guid.NewGuid(),
                Status = OrderStatus.Completed,
                ChangedByUserId = operatorUserId,
                ChangedAt = DateTime.UtcNow
            });
            await _orderRepo.UpdateAsync(order);

            await _unitOfWork.CommitAsync();
            return (true, null, false);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private static (List<OrderItem>? Items, string? Error) BuildItems(IEnumerable<OrderItemRequest> itemRequests, IReadOnlyList<Product> products)
    {
        var items = new List<OrderItem>();
        foreach (var request in itemRequests)
        {
            var product = products.FirstOrDefault(p => p.Id == request.ProductId);
            if (product == null)
                return (null, $"Product with id: {request.ProductId} does not exist");
            if (!product.IsActive)
                return (null, $"Product with id: {request.ProductId} is inactive");
            if (items.Any(i => i.ProductId == request.ProductId))
                return (null, $"Product with id: {request.ProductId} is duplicated in the order");

            items.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = product.Price
            });
        }
        return (items, null);
    }
}