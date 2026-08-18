using InventoryManagementSystem.Application.DTOs.Operators;
using InventoryManagementSystem.Application.DTOs.Users;
using InventoryManagementSystem.Application.Services;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/warehouses/{warehouseId:guid}/operators")]
[Authorize(Policy = "Warehouse.Edit")]
public class OperatorsController : ControllerBase
{
    private readonly IOperatorService _operatorService;

    public OperatorsController(IOperatorService operatorService) => _operatorService = operatorService;

    [HttpGet("/api/operators")]
    public async Task<IActionResult> GetAll()
    {
        var operators = await _operatorService.GetAllAsync();
        return Ok(operators.Select(ToResponse));
    }

    [HttpGet]
    public async Task<IActionResult> GetByWarehouse(Guid warehouseId)
    {
        var operators = await _operatorService.GetByWarehouseAsync(warehouseId);
        return Ok(operators.Select(ToResponse));
    }

    [HttpPost]
    public async Task<IActionResult> Assign(Guid warehouseId, [FromBody] AssignOperatorRequest request)
    {
        var assign = new WarehouseOperator
        {
            Id = Guid.NewGuid(),
            WarehouseId = warehouseId,
            OperatorUserId = request.OperatorUserId
        };
        try
        {
            await _operatorService.AddAsync(assign);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        var created = await _operatorService.GetByWarehouseAndUserAsync(assign.WarehouseId, assign.OperatorUserId);
        return CreatedAtAction(nameof(GetByWarehouse), new { warehouseId }, ToResponse(created!));
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> Update(Guid warehouseId, Guid userId, [FromBody] UpdateOperatorRequest request)
    {
        var current = await _operatorService.GetByWarehouseAndUserAsync(warehouseId, userId);
        if (current == null) return NotFound(new { message = "Assignment not found" });

        var assign = new WarehouseOperator
        {
            Id = current.Id,
            WarehouseId = request.WarehouseId ?? current.WarehouseId,
            OperatorUserId = request.OperatorUserId ?? current.OperatorUserId
        };
        try
        {
            await _operatorService.UpdateAsync(assign);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        var updated = await _operatorService.GetByWarehouseAndUserAsync(assign.WarehouseId, assign.OperatorUserId);
        return Ok(ToResponse(updated!));
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> Remove(Guid warehouseId, Guid userId)
    {
        await _operatorService.RemoveAsync(warehouseId, userId);
        return NoContent();
    }

    private static OperatorResponse ToResponse(WarehouseOperator assign) =>
        new(assign.Id, assign.WarehouseId, new UserResponse(
            assign.Operator.Id,
            assign.Operator.Username,
            assign.Operator.Email,
            assign.Operator.DisplayName,
            assign.Operator.Role?.Name ?? "Unknown",
            assign.Operator.IsActive,
            assign.Operator.CreatedAt));
}