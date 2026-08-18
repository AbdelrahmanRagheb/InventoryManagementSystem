using System.Security.Claims;
using InventoryManagementSystem.Application.Authorization;
using InventoryManagementSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InventoryManagementSystem.API.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IAccessService _accessService;

    public PermissionAuthorizationHandler(IAccessService accessService)
    {
        _accessService = accessService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userIdValue = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var userId))
            return;

        if (!PermissionCatalog.IsWarehouseScoped(requirement.Permission))
        {
            if (await _accessService.HasPermissionAsync(userId, requirement.Permission))
                context.Succeed(requirement);
            return;
        }

        if (context.Resource is AuthorizationFilterContext filterContext)
        {
            var routeValues = filterContext.RouteData.Values;
            var warehouseValue = (routeValues.TryGetValue("warehouseId", out var wh) ? wh?.ToString()
                : routeValues.TryGetValue("id", out var id) ? id?.ToString()
                : null);
            if (Guid.TryParse(warehouseValue, out var warehouseId))
            {
                if (await _accessService.CanAsync(userId, requirement.Permission, warehouseId))
                    context.Succeed(requirement);
                return;
            }
        }

        if (await _accessService.CanAsync(userId, requirement.Permission, null))
            context.Succeed(requirement);
    }
}