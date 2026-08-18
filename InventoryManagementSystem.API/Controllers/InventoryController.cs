using System.Security.Claims;
using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Application.DTOs.Inventory;
using InventoryManagementSystem.Application.Services;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService) => _inventoryService = inventoryService;

    [HttpGet]
    [Authorize(Policy = "Inventory.View")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = Paging.DefaultPageSize)
    {
        var inventories = await _inventoryService.GetAllPagedAsync(page, pageSize, CurrentUserId());
        return Ok(new PagedResponse<InventoryResponse>(
            inventories.Items.Select(ToResponse).ToList(),
            inventories.Page,
            inventories.PageSize,
            inventories.TotalCount));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "Inventory.View")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var (inventory, forbidden) = await _inventoryService.GetByIdAsync(id, CurrentUserId());
        if (forbidden) return Forbid();
        if (inventory == null) return NotFound();
        return Ok(ToResponse(inventory));
    }

    [HttpGet("~/api/products/{productId:guid}/inventory")]
    [Authorize(Policy = "Inventory.View")]
    public async Task<IActionResult> GetByProduct(Guid productId, [FromQuery] int page = 1, [FromQuery] int pageSize = Paging.DefaultPageSize)
    {
        var inventories = await _inventoryService.GetByProductPagedAsync(productId, page, pageSize, CurrentUserId());
        return Ok(new PagedResponse<InventoryResponse>(
            inventories.Items.Select(ToResponse).ToList(),
            inventories.Page,
            inventories.PageSize,
            inventories.TotalCount));
    }

    [HttpGet("~/api/warehouses/{warehouseId:guid}/inventory")]
    [Authorize(Policy = "Inventory.View")]
    public async Task<IActionResult> GetByWarehouse(Guid warehouseId, [FromQuery] int page = 1, [FromQuery] int pageSize = Paging.DefaultPageSize)
    {
        var (inventories, forbidden) = await _inventoryService.GetByWarehousePagedAsync(warehouseId, page, pageSize, CurrentUserId());
        if (forbidden) return Forbid();
        return Ok(new PagedResponse<InventoryResponse>(
            inventories!.Items.Select(ToResponse).ToList(),
            inventories.Page,
            inventories.PageSize,
            inventories.TotalCount));
    }

    [HttpPost]
    [Route("adjust")]
    [Authorize]
    public async Task<IActionResult> Adjust([FromBody] AdjustStockRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var changedByUserId))
            return Unauthorized();

        var (success, error, forbidden) = await _inventoryService.AdjustAsync(request, changedByUserId);
        if (forbidden) return Forbid();
        if (!success) return BadRequest(new { error });
        return Ok();
    }

    private Guid CurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : Guid.Empty;
    }

    private static InventoryResponse ToResponse(Inventory inventory) =>
        new(inventory.Id, inventory.ProductId, inventory.WarehouseId, inventory.Quantity, inventory.UpdatedAt);
}