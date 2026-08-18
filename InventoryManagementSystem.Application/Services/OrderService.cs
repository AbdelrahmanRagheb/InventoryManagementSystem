using InventoryManagementSystem.Application.Authorization;
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
    private readonly IAccessService _accessService;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        IOrderRepository orderRepo,
        IProductRepository productRepo,
        IInventoryRepository inventoryRepo,
        IInventoryTransactionRepository transactionRepo,
        IAccessService accessService,
        IUnitOfWork unitOfWork)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
        _inventoryRepo = inventoryRepo;
        _transactionRepo = transactionRepo;
        _accessService = accessService;
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
        var updateRole = await _accessService.GetRoleNameAsync(userId);
        if (order.CreatedByUserId != userId && updateRole != RoleDefaults.Admin)
            return (false, null, true);
        if (order.Status != OrderStatus.Pending)
            return (false, "Only pending orders can be updated", false);

        if (request.CustomerName != null) order.CustomerName = request.CustomerName;
        if (request.CustomerEmail != null) order.CustomerEmail = request.CustomerEmail;

        if (request.Items != null && request.Items.Count > 0)
        {
            if (!await _accessService.HasPermissionAsync(userId, PermissionCatalog.OrderItemAdd) ||
                !await _accessService.HasPermissionAsync(userId, PermissionCatalog.OrderItemRemove))
                return (false, null, true);

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
        var cancelRole = await _accessService.GetRoleNameAsync(userId);
        if (order.CreatedByUserId != userId && cancelRole != RoleDefaults.Admin)
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

    public async Task<(bool Success, string? Error, bool Forbidden)> FulfillAsync(Guid orderId, FulfillOrderRequest request, Guid operatorUserId)
    {
        var order = await _orderRepo.GetByIdWithItemsAsync(orderId);
        if (order == null)
            return (false, $"Order with id: {orderId} does not exist", false);
        if (order.Status != OrderStatus.Pending)
            return (false, "Only pending orders can be fulfilled", false);

        var warehousesByItem = new Dictionary<Guid, Guid>();
        foreach (var entry in request.Items)
        {
            if (warehousesByItem.ContainsKey(entry.OrderItemId))
                return (false, $"Order item: {entry.OrderItemId} is assigned more than once", false);
            warehousesByItem[entry.OrderItemId] = entry.WarehouseId;
        }

        foreach (var item in order.Items)
        {
            if (!warehousesByItem.ContainsKey(item.Id))
                return (false, $"Warehouse not specified for order item: {item.Id}", false);
        }

        foreach (var itemId in warehousesByItem.Keys)
        {
            if (order.Items.All(i => i.Id != itemId))
                return (false, $"Order item: {itemId} does not belong to this order", false);
        }

        foreach (var warehouseId in warehousesByItem.Values.Distinct())
        {
            if (!await _accessService.CanAsync(operatorUserId, PermissionCatalog.OrderComplete, warehouseId))
                return (false, "You are only allowed to fulfill orders from warehouses you are assigned to complete", true);
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            foreach (var item in order.Items)
            {
                var warehouseId = warehousesByItem[item.Id];
                var inventory = await _inventoryRepo.GetByProductWarehouseAsync(item.ProductId, warehouseId);
                if (inventory == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, $"No inventory record for product: {item.Product.Name} in this warehouse", false);
                }
                if (inventory.Quantity < item.Quantity)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, $"Insufficient stock for product: {item.Product.Name} in this warehouse", false);
                }

                var previous = inventory.Quantity;
                inventory.Quantity = previous - item.Quantity;
                inventory.UpdatedAt = DateTime.UtcNow;
                await _inventoryRepo.UpdateAsync(inventory);

                item.WarehouseId = warehouseId;

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