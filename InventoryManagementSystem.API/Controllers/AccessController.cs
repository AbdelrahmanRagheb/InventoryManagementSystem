using System.Security.Claims;
using InventoryManagementSystem.Application.DTOs.Access;
using InventoryManagementSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/access")]
[Authorize]
public class AccessController : ControllerBase
{
    private readonly IAccessManagementService _accessManagement;

    public AccessController(IAccessManagementService accessManagement) => _accessManagement = accessManagement;

    [HttpGet("permissions")]
    [Authorize(Policy = "User.ManagePermissions")]
    public async Task<IActionResult> GetPermissions() =>
        Ok(await _accessManagement.GetPermissionsAsync());

    [HttpGet("roles")]
    [Authorize(Policy = "User.ManagePermissions")]
    public async Task<IActionResult> GetRoles() =>
        Ok(await _accessManagement.GetRolesAsync());

    [HttpGet("users")]
    [Authorize(Policy = "User.ManagePermissions")]
    public async Task<IActionResult> GetUsers() =>
        Ok(await _accessManagement.GetUsersAsync());

    [HttpGet("resources")]
    [Authorize(Policy = "User.ManagePermissions")]
    public async Task<IActionResult> GetResources([FromQuery] string? type) =>
        Ok(await _accessManagement.GetResourcesAsync(type));

    [HttpPut("users/{userId:guid}/role")]
    [Authorize(Policy = "User.AssignRole")]
    public async Task<IActionResult> AssignRole(Guid userId, [FromBody] AssignRoleRequest request)
    {
        if (userId == CurrentUserId())
            return Forbid();

        var (success, error, user) = await _accessManagement.AssignRoleAsync(userId, request.RoleId);
        if (!success) return NotFound(new { error });
        return Ok(user);
    }

    [HttpPost("users/{userId:guid}/permissions")]
    [Authorize(Policy = "User.ManagePermissions")]
    public async Task<IActionResult> Grant(Guid userId, [FromBody] GrantPermissionRequest request)
    {
        var (success, error, grant) = await _accessManagement.GrantAsync(userId, request.PermissionId, request.WarehouseId);
        if (!success)
        {
            if (error == "Permission not found" || error == "Warehouse has no linked resource")
                return BadRequest(new { error });
            return NotFound(new { error });
        }
        return Ok(grant);
    }

    [HttpDelete("users/{userId:guid}/permissions/{userPermissionId:guid}")]
    [Authorize(Policy = "User.ManagePermissions")]
    public async Task<IActionResult> Revoke(Guid userId, Guid userPermissionId)
    {
        var owner = await _accessManagement.GetGrantOwnerAsync(userPermissionId);
        if (owner == null)
            return NotFound(new { message = "Grant not found" });
        if (owner == CurrentUserId())
            return Forbid();

        await _accessManagement.RevokeAsync(userPermissionId);
        return NoContent();
    }

    private Guid CurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : Guid.Empty;
    }
}