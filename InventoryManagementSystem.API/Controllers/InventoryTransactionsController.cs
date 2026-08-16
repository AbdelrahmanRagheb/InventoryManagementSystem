using InventoryManagementSystem.Application.DTOs.Inventory;
using InventoryManagementSystem.Application.Services;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/inventory/transactions")]
[Authorize(Roles = "Admin,Manager,WarehouseOperator")]
public class InventoryTransactionsController : ControllerBase
{
    private readonly IInventoryTransactionService _transactionService;

    public InventoryTransactionsController(IInventoryTransactionService transactionService) => _transactionService = transactionService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var transactions = await _transactionService.GetAllAsync();
        return Ok(transactions.Select(ToResponse));
    }

    [HttpGet("product/{productId:guid}")]
    public async Task<IActionResult> GetByProduct(Guid productId)
    {
        var transactions = await _transactionService.GetByProductAsync(productId);
        return Ok(transactions.Select(ToResponse));
    }

    [HttpGet("warehouse/{warehouseId:guid}")]
    public async Task<IActionResult> GetByWarehouse(Guid warehouseId)
    {
        var transactions = await _transactionService.GetByWarehouseAsync(warehouseId);
        return Ok(transactions.Select(ToResponse));
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