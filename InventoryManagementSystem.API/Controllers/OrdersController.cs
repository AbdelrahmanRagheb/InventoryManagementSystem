using System.Linq;
using System.Security.Claims;
using InventoryManagementSystem.Application.DTOs.Orders;
using InventoryManagementSystem.Application.Services;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders.Select(ToResponse));
    }

    [HttpGet("mine")]
    [Authorize(Roles = "SalesAgent")]
    public async Task<IActionResult> GetMine()
    {
        var orders = await _orderService.GetByUserAsync(CurrentUserId());
        return Ok(orders.Select(ToResponse));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _orderService.GetByIdWithItemsAsync(id);
        if (order == null)
            return NotFound(new { message = $"Order with id: {id} does not exist" });

        if (User.IsInRole("SalesAgent") && order.CreatedByUserId != CurrentUserId())
            return Forbid();

        return Ok(ToResponse(order));
    }

    [HttpPost]
    [Authorize(Roles = "SalesAgent")]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var (success, created, error) = await _orderService.CreateAsync(request, CurrentUserId());
        if (!success)
            return BadRequest(new { error });

        var order = await _orderService.GetByIdWithItemsAsync(created!.Id);
        return CreatedAtAction(nameof(GetById), new { id = order!.Id }, ToResponse(order));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "SalesAgent")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderRequest request)
    {
        var (success, error, forbidden) = await _orderService.UpdateAsync(id, request, CurrentUserId());
        if (forbidden)
            return Forbid();
        if (!success)
            return BadRequest(new { error });

        var order = await _orderService.GetByIdWithItemsAsync(id);
        return Ok(ToResponse(order!));
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize(Roles = "SalesAgent")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var (success, error, forbidden) = await _orderService.CancelAsync(id, CurrentUserId());
        if (forbidden)
            return Forbid();
        if (!success)
            return BadRequest(new { error });

        var order = await _orderService.GetByIdWithItemsAsync(id);
        return Ok(ToResponse(order!));
    }

    [HttpPost("{id:guid}/fulfill")]
    [Authorize(Roles = "WarehouseOperator")]
    public async Task<IActionResult> Fulfill(Guid id, [FromBody] FulfillOrderRequest request)
    {
        var (success, error, forbidden) = await _orderService.FulfillAsync(id, request.WarehouseId, CurrentUserId());
        if (forbidden)
            return Forbid();
        if (!success)
            return BadRequest(new { error });

        var order = await _orderService.GetByIdWithItemsAsync(id);
        return Ok(ToResponse(order!));
    }

    private Guid CurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : Guid.Empty;
    }

    private static OrderResponse ToResponse(Order order) =>
        new(
            order.Id,
            order.CustomerName,
            order.CustomerEmail,
            order.Status,
            order.CreatedByUserId,
            order.CreatedAt,
            order.CompletedAt,
            order.CancelledAt,
            order.Items.Sum(i => i.Quantity * i.UnitPrice),
            order.Items.Select(i => new OrderItemResponse(i.Id, i.ProductId, i.Product.Name, i.Quantity, i.UnitPrice)).ToList(),
            order.History.Select(h => new OrderHistoryResponse(h.Id, h.Status, h.ChangedByUserId, h.ChangedAt)).ToList());
}