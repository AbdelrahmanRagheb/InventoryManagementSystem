using InventoryManagementSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService) => _reportService = reportService;

    [HttpGet("orders")]
    [Authorize(Policy = "Report.ViewOrders")]
    public async Task<IActionResult> GetOrderSummary() =>
        Ok(await _reportService.GetOrderSummaryAsync());

    [HttpGet("inventory")]
    [Authorize(Policy = "Report.ViewInventory")]
    public async Task<IActionResult> GetInventorySummary([FromQuery] int below = 10) =>
        Ok(await _reportService.GetInventorySummaryAsync(below));

    [HttpGet("transactions")]
    [Authorize(Policy = "Report.ViewTransactions")]
    public async Task<IActionResult> GetTransactionSummary() =>
        Ok(await _reportService.GetTransactionSummaryAsync());

    [HttpGet("warehouses")]
    [Authorize(Policy = "Report.ViewWarehouseSummary")]
    public async Task<IActionResult> GetWarehouseSummary() =>
        Ok(await _reportService.GetWarehouseSummaryAsync());
}
