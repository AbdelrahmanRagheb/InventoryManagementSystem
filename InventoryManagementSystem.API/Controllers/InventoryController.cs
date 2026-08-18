using System.Security.Claims;
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
    public async Task<IActionResult> GetAll()
    {
        var inventories = await _inventoryService.GetAllAsync();
        return Ok(inventories.Select(ToResponse));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "Inventory.View")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var inventory = await _inventoryService.GetByIdAsync(id);
        if (inventory == null) return NotFound();
        return Ok(ToResponse(inventory));
    }

    [HttpGet("~/api/products/{productId:guid}/inventory")]
    [Authorize(Policy = "Inventory.View")]
    public async Task<IActionResult> GetByProduct(Guid productId)
    {
        var inventories = await _inventoryService.GetByProductAsync(productId);
        return Ok(inventories.Select(ToResponse));
    }

    [HttpGet("~/api/warehouses/{warehouseId:guid}/inventory")]
    [Authorize(Policy = "Inventory.View")]
    public async Task<IActionResult> GetByWarehouse(Guid warehouseId)
    {
        var inventories = await _inventoryService.GetByWarehouseAsync(warehouseId);
        return Ok(inventories.Select(ToResponse));
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

    private static InventoryResponse ToResponse(Inventory inventory) =>
        new(inventory.Id, inventory.ProductId, inventory.WarehouseId, inventory.Quantity, inventory.UpdatedAt);
}