using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryManagementSystem.Application.DTOs.Reports;

namespace InventoryManagementSystem.Application.Services;

public interface IReportService
{
    Task<OrderSummaryResponse> GetOrderSummaryAsync();
    Task<InventorySummaryResponse> GetInventorySummaryAsync(int lowStockThreshold);
    Task<TransactionSummaryResponse> GetTransactionSummaryAsync();
    Task<IReadOnlyList<WarehouseSummaryResponse>> GetWarehouseSummaryAsync();
}
