using System.Security.Claims;
using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Application.DTOs.Inventory;
using InventoryManagementSystem.Application.Services;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/inventory/transactions")]
[Authorize(Policy = "Inventory.View")]
public class InventoryTransactionsController : ControllerBase
{
    private readonly IInventoryTransactionService _transactionService;

    public InventoryTransactionsController(IInventoryTransactionService transactionService) => _transactionService = transactionService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = Paging.DefaultPageSize)
    {
        var transactions = await _transactionService.GetAllPagedAsync(page, pageSize, CurrentUserId());
        return Ok(new PagedResponse<InventoryTransactionResponse>(
            transactions.Items.Select(ToResponse).ToList(),
            transactions.Page,
            transactions.PageSize,
            transactions.TotalCount));
    }

    [HttpGet("product/{productId:guid}")]
    public async Task<IActionResult> GetByProduct(Guid productId, [FromQuery] int page = 1, [FromQuery] int pageSize = Paging.DefaultPageSize)
    {
        var transactions = await _transactionService.GetByProductPagedAsync(productId, page, pageSize, CurrentUserId());
        return Ok(new PagedResponse<InventoryTransactionResponse>(
            transactions.Items.Select(ToResponse).ToList(),
            transactions.Page,
            transactions.PageSize,
            transactions.TotalCount));
    }

    [HttpGet("warehouse/{warehouseId:guid}")]
    public async Task<IActionResult> GetByWarehouse(Guid warehouseId, [FromQuery] int page = 1, [FromQuery] int pageSize = Paging.DefaultPageSize)
    {
        var (transactions, forbidden) = await _transactionService.GetByWarehousePagedAsync(warehouseId, page, pageSize, CurrentUserId());
        if (forbidden) return Forbid();
        return Ok(new PagedResponse<InventoryTransactionResponse>(
            transactions!.Items.Select(ToResponse).ToList(),
            transactions.Page,
            transactions.PageSize,
            transactions.TotalCount));
    }

    private Guid CurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : Guid.Empty;
    }

    private static InventoryTransactionResponse ToResponse(InventoryTransaction transaction) =>
        new(
            transaction.Id,
            transaction.ProductId,
            transaction.WarehouseId,
            (int)transaction.Type,
            transaction.QuantityChange,
            transaction.PreviousQuantity,
            transaction.NewQuantity,
            transaction.Reason,
            transaction.CreatedByUserId,
            transaction.CreatedAt);
}