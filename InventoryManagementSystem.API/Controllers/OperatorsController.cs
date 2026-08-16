using InventoryManagementSystem.Application.DTOs.Operators;
using InventoryManagementSystem.Application.Services;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/warehouses/{warehouseId:guid}/operators")]
[Authorize(Roles = "Admin")]
public class OperatorsController : ControllerBase
{
    private readonly IOperatorService _operatorService;

    public OperatorsController(IOperatorService operatorService) => _operatorService = operatorService;

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
        return CreatedAtAction(nameof(GetByWarehouse), new { warehouseId }, ToResponse(assign));
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> Remove(Guid warehouseId, Guid userId)
    {
        await _operatorService.RemoveAsync(warehouseId, userId);
        return NoContent();
    }

    private static OperatorResponse ToResponse(WarehouseOperator assign) =>
        new(assign.Id, assign.WarehouseId, assign.OperatorUserId);
}