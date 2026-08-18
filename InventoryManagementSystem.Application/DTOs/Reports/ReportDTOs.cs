using System.Collections.Generic;
using InventoryManagementSystem.Application.DTOs.Inventory;
using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Application.DTOs.Reports;

public record OrderSummaryResponse(
    int TotalOrders,
    int PendingCount,
    int CompletedCount,
    int CancelledCount,
    decimal TotalRevenue,
    decimal AverageOrderValue,
    IReadOnlyList<RecentOrderResponse> RecentOrders);

public record RecentOrderResponse(
    Guid Id,
    string CustomerName,
    OrderStatus Status,
    decimal TotalAmount,
    DateTime CreatedAt);

public record InventorySummaryResponse(
    int TotalSkuCount,
    long TotalUnits,
    decimal TotalStockValue,
    int LowStockCount,
    IReadOnlyList<WarehouseStockResponse> ByWarehouse);

public record WarehouseStockResponse(
    Guid WarehouseId,
    string WarehouseName,
    int SkuCount,
    long TotalUnits,
    decimal StockValue);

public record TransactionSummaryResponse(
    int TotalTransactions,
    int InitialCount,
    int IncreaseCount,
    int DecreaseCount,
    long TotalIncreaseUnits,
    long TotalDecreaseUnits,
    IReadOnlyList<InventoryTransactionResponse> RecentTransactions);

public record WarehouseSummaryResponse(
    Guid WarehouseId,
    string WarehouseName,
    int SkuCount,
    long TotalUnits,
    decimal StockValue,
    int ActiveOperatorCount,
    int CompletedOrderCount);
