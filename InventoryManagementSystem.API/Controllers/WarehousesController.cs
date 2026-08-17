using InventoryManagementSystem.Application.DTOs.Warehouses;
using InventoryManagementSystem.Application.Services;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehousesController(IWarehouseService warehouseService) => _warehouseService = warehouseService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var warehouses = await _warehouseService.GetAllAsync();
        return Ok(warehouses.Select(ToResponse));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse == null) return NotFound();
        return Ok(ToResponse(warehouse));
    }

    [HttpPost]
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
    public async Task<IActionResult> DeactivateWarehouse(Guid id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse == null) return NotFound(new { message = $"Warehouse with id: {id} does not exist" });
        await _warehouseService.DeactivateAsync(id);
        return Ok(ToResponse(warehouse));
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var warehouse = await _warehouseService.ActivateAsync(id);
        if (warehouse == null) return NotFound(new { message = $"Warehouse with id: {id} does not exist" });
        return Ok(ToResponse(warehouse));
    }

    private static WarehouseResponse ToResponse(Warehouse warehouse) =>
        new(warehouse.Id, warehouse.Name, warehouse.Location, warehouse.IsActive, warehouse.CreatedAt);
}