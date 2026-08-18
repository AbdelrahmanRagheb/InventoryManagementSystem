using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagementSystem.Application.DTOs.Inventory;
using InventoryManagementSystem.Application.DTOs.Reports;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Application.Services;

public class ReportService : IReportService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IProductRepository _productRepo;
    private readonly IInventoryTransactionRepository _transactionRepo;
    private readonly IWarehouseRepository _warehouseRepo;
    private readonly IWarehouseOperatorRepository _operatorRepo;

    public ReportService(
        IOrderRepository orderRepo,
        IInventoryRepository inventoryRepo,
        IProductRepository productRepo,
        IInventoryTransactionRepository transactionRepo,
        IWarehouseRepository warehouseRepo,
        IWarehouseOperatorRepository operatorRepo)
    {
        _orderRepo = orderRepo;
        _inventoryRepo = inventoryRepo;
        _productRepo = productRepo;
        _transactionRepo = transactionRepo;
        _warehouseRepo = warehouseRepo;
        _operatorRepo = operatorRepo;
    }

    public async Task<OrderSummaryResponse> GetOrderSummaryAsync()
    {
        var orders = await _orderRepo.GetAllWithItemsAsync();

        var totalOrders = orders.Count;
        var totalRevenue = orders
            .Where(o => o.Status == OrderStatus.Completed)
            .Sum(o => o.Items.Sum(i => i.Quantity * i.UnitPrice));
        var averageOrderValue = totalOrders > 0
            ? orders.Average(o => o.Items.Sum(i => i.Quantity * i.UnitPrice))
            : 0;

        var recentOrders = orders
            .OrderByDescending(o => o.CreatedAt)
            .Take(10)
            .Select(o => new RecentOrderResponse(
                o.Id,
                o.CustomerName,
                o.Status,
                o.Items.Sum(i => i.Quantity * i.UnitPrice),
                o.CreatedAt))
            .ToList();

        return new OrderSummaryResponse(
            totalOrders,
            orders.Count(o => o.Status == OrderStatus.Pending),
            orders.Count(o => o.Status == OrderStatus.Completed),
            orders.Count(o => o.Status == OrderStatus.Cancelled),
            totalRevenue,
            averageOrderValue,
            recentOrders);
    }

    public async Task<InventorySummaryResponse> GetInventorySummaryAsync(int lowStockThreshold)
    {
        var inventories = await _inventoryRepo.GetAllAsync();
        var products = await _productRepo.GetAllAsync();
        var warehouses = await _warehouseRepo.GetAllAsync();

        var priceByProduct = products.ToDictionary(p => p.Id, p => p.Price);
        var stockByProduct = inventories
            .GroupBy(i => i.ProductId)
            .ToDictionary(g => g.Key, g => g.Sum(i => (long)i.Quantity));

        var totalUnits = stockByProduct.Values.Sum();
        var totalStockValue = inventories.Sum(i => (long)i.Quantity * PriceOrZero(priceByProduct, i.ProductId));
        var lowStockCount = stockByProduct.Count(kv => kv.Value < lowStockThreshold);

        var byWarehouse = warehouses
            .Select(w =>
            {
                var warehouseInventory = inventories.Where(i => i.WarehouseId == w.Id).ToList();
                return new WarehouseStockResponse(
                    w.Id,
                    w.Name,
                    warehouseInventory.Select(i => i.ProductId).Distinct().Count(),
                    warehouseInventory.Sum(i => (long)i.Quantity),
                    warehouseInventory.Sum(i => (long)i.Quantity * PriceOrZero(priceByProduct, i.ProductId)));
            })
            .ToList();

        return new InventorySummaryResponse(
            products.Count,
            totalUnits,
            totalStockValue,
            lowStockCount,
            byWarehouse);
    }

    public async Task<TransactionSummaryResponse> GetTransactionSummaryAsync()
    {
        var transactions = await _transactionRepo.GetAllAsync();

        var recentTransactions = transactions
            .OrderByDescending(t => t.CreatedAt)
            .Take(10)
            .Select(t => new InventoryTransactionResponse(
                t.Id,
                t.ProductId,
                t.WarehouseId,
                (int)t.Type,
                t.QuantityChange,
                t.PreviousQuantity,
                t.NewQuantity,
                t.Reason,
                t.CreatedByUserId,
                t.CreatedAt))
            .ToList();

        return new TransactionSummaryResponse(
            transactions.Count,
            transactions.Count(t => t.Type == TransactionType.Initial),
            transactions.Count(t => t.Type == TransactionType.Increase),
            transactions.Count(t => t.Type == TransactionType.Decrease),
            transactions.Where(t => t.Type == TransactionType.Increase).Sum(t => (long)t.QuantityChange),
            transactions.Where(t => t.Type == TransactionType.Decrease).Sum(t => (long)t.QuantityChange),
            recentTransactions);
    }

    public async Task<IReadOnlyList<WarehouseSummaryResponse>> GetWarehouseSummaryAsync()
    {
        var warehouses = await _warehouseRepo.GetAllAsync();
        var inventories = await _inventoryRepo.GetAllAsync();
        var products = await _productRepo.GetAllAsync();
        var orders = await _orderRepo.GetAllWithItemsAsync();

        var priceByProduct = products.ToDictionary(p => p.Id, p => p.Price);
        var result = new List<WarehouseSummaryResponse>();

        foreach (var warehouse in warehouses)
        {
            var warehouseInventory = inventories.Where(i => i.WarehouseId == warehouse.Id).ToList();
            var operators = await _operatorRepo.GetByWarehouseAsync(warehouse.Id);
            var completedOrders = orders.Count(o =>
                o.Status == OrderStatus.Completed &&
                o.Items.Any(i => i.WarehouseId == warehouse.Id));

            result.Add(new WarehouseSummaryResponse(
                warehouse.Id,
                warehouse.Name,
                warehouseInventory.Select(i => i.ProductId).Distinct().Count(),
                warehouseInventory.Sum(i => (long)i.Quantity),
                warehouseInventory.Sum(i => (long)i.Quantity * PriceOrZero(priceByProduct, i.ProductId)),
                operators.Count,
                completedOrders));
        }

        return result;
    }

    private static decimal PriceOrZero(IReadOnlyDictionary<Guid, decimal> priceByProduct, Guid productId) =>
        priceByProduct.TryGetValue(productId, out var price) ? price : 0;
}
