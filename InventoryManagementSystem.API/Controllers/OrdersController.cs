using System.Linq;
using System.Security.Claims;
using InventoryManagementSystem.Application.DTOs.Common;
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
    private readonly IAccessService _accessService;

    public OrdersController(IOrderService orderService, IAccessService accessService)
    {
        _orderService = orderService;
        _accessService = accessService;
    }

    [HttpGet]
    [Authorize(Policy = "Order.View")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = Paging.DefaultPageSize)
    {
        var orders = await _orderService.GetAllPagedAsync(page, pageSize, CurrentUserId());
        return Ok(new PagedResponse<OrderResponse>(
            orders.Items.Select(ToResponse).ToList(),
            orders.Page,
            orders.PageSize,
            orders.TotalCount));
    }

    [HttpGet("mine")]
    [Authorize(Policy = "Order.ViewOwn")]
    public async Task<IActionResult> GetMine([FromQuery] int page = 1, [FromQuery] int pageSize = Paging.DefaultPageSize)
    {
        var orders = await _orderService.GetByUserPagedAsync(page, pageSize, CurrentUserId());
        return Ok(new PagedResponse<OrderResponse>(
            orders.Items.Select(ToResponse).ToList(),
            orders.Page,
            orders.PageSize,
            orders.TotalCount));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = CurrentUserId();
        var (order, forbidden) = await _orderService.GetByIdWithItemsAsync(id, userId);
        if (forbidden)
            return Forbid();
        if (order == null)
            return NotFound(new { message = $"Order with id: {id} does not exist" });

        if (await _accessService.HasPermissionAsync(userId, "Order.View"))
            return Ok(ToResponse(order));

        if (order.CreatedByUserId == userId &&
            await _accessService.HasPermissionAsync(userId, "Order.ViewOwn"))
            return Ok(ToResponse(order));

        return Forbid();
    }

    [HttpPost]
    [Authorize(Policy = "Order.Create")]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var (success, created, error) = await _orderService.CreateAsync(request, CurrentUserId());
        if (!success)
            return BadRequest(new { error });

        var (order, _) = await _orderService.GetByIdWithItemsAsync(created!.Id, CurrentUserId());
        return CreatedAtAction(nameof(GetById), new { id = order!.Id }, ToResponse(order));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Order.Edit")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderRequest request)
    {
        var (success, error, forbidden) = await _orderService.UpdateAsync(id, request, CurrentUserId());
        if (forbidden)
            return Forbid();
        if (!success)
            return BadRequest(new { error });

        var (order, _) = await _orderService.GetByIdWithItemsAsync(id, CurrentUserId());
        return Ok(ToResponse(order!));
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize(Policy = "Order.Cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var (success, error, forbidden) = await _orderService.CancelAsync(id, CurrentUserId());
        if (forbidden)
            return Forbid();
        if (!success)
            return BadRequest(new { error });

        var (order, _) = await _orderService.GetByIdWithItemsAsync(id, CurrentUserId());
        return Ok(ToResponse(order!));
    }

    [HttpPost("{id:guid}/fulfill")]
    [Authorize]
    public async Task<IActionResult> Fulfill(Guid id, [FromBody] FulfillOrderRequest request)
    {
        var (success, error, forbidden) = await _orderService.FulfillAsync(id, request, CurrentUserId());
        if (forbidden)
            return Forbid();
        if (!success)
            return BadRequest(new { error });

        var (order, _) = await _orderService.GetByIdWithItemsAsync(id, CurrentUserId());
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
            order.Items.Select(i => new OrderItemResponse(i.Id, i.ProductId, i.Product.Name, i.Quantity, i.UnitPrice, i.WarehouseId)).ToList(),
            order.History.Select(h => new OrderHistoryResponse(h.Id, h.Status, h.ChangedByUserId, h.ChangedAt)).ToList());
}