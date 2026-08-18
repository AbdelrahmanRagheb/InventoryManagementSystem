using System.Security.Claims;
using InventoryManagementSystem.Application.DTOs.Common;
using InventoryManagementSystem.Application.DTOs.Warehouses;
using InventoryManagementSystem.Application.Services;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;
    private readonly IAccessService _accessService;

    public WarehousesController(IWarehouseService warehouseService, IAccessService accessService)
    {
        _warehouseService = warehouseService;
        _accessService = accessService;
    }

    [HttpGet]
    [Authorize(Policy = "Warehouse.View")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = Paging.DefaultPageSize)
    {
        var warehouses = await _warehouseService.GetPagedVisibleAsync(page, pageSize, CurrentUserId());
        return Ok(new PagedResponse<WarehouseResponse>(
            warehouses.Items.Select(ToResponse).ToList(),
            warehouses.Page,
            warehouses.PageSize,
            warehouses.TotalCount));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "Warehouse.View")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = CurrentUserId();
        if (await _accessService.IsRestrictedToAssignedWarehousesAsync(userId))
        {
            var assigned = await _accessService.GetAssignedWarehouseIdsAsync(userId);
            if (!assigned.Contains(id)) return Forbid();
        }
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse == null) return NotFound();
        return Ok(ToResponse(warehouse));
    }

    [HttpPost]
    [Authorize(Policy = "Warehouse.Create")]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseRequest request)
    {
        var warehouse = new Warehouse
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Location = request.Location,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _warehouseService.AddAsync(warehouse);
        return CreatedAtAction(nameof(GetById), new { id = warehouse.Id }, ToResponse(warehouse));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Warehouse.Edit")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWarehouseRequest request)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse == null) return NotFound();
        if (request.Name != null) warehouse.Name = request.Name;
        if (request.Location != null) warehouse.Location = request.Location;
        await _warehouseService.UpdateAsync(warehouse);
        return Ok(ToResponse(warehouse));
    }

    [HttpPost("{id:guid}/deactivate")]
    [Authorize(Policy = "Warehouse.Deactivate")]
    public async Task<IActionResult> DeactivateWarehouse(Guid id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse == null) return NotFound(new { message = $"Warehouse with id: {id} does not exist" });
        await _warehouseService.DeactivateAsync(id);
        return Ok(ToResponse(warehouse));
    }

    [HttpPost("{id:guid}/activate")]
    [Authorize(Policy = "Warehouse.Activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var warehouse = await _warehouseService.ActivateAsync(id);
        if (warehouse == null) return NotFound(new { message = $"Warehouse with id: {id} does not exist" });
        return Ok(ToResponse(warehouse));
    }

    private Guid CurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : Guid.Empty;
    }

    private static WarehouseResponse ToResponse(Warehouse warehouse) =>
        new(warehouse.Id, warehouse.Name, warehouse.Location, warehouse.IsActive, warehouse.CreatedAt);
}