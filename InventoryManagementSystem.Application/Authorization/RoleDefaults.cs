using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.Application.Authorization;

public static class RoleDefaults
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string WarehouseOperator = "WarehouseOperator";
    public const string SalesAgent = "SalesAgent";

    public static readonly IReadOnlyList<string> AllRoleNames = new[]
    {
        Admin, Manager, WarehouseOperator, SalesAgent
    };

    public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Permissions =
        new Dictionary<string, IReadOnlyList<string>>
        {
            [Admin] = PermissionCatalog.All.ToList(),
            [SalesAgent] = new[]
            {
                PermissionCatalog.ProductView,
                PermissionCatalog.OrderCreate,
                PermissionCatalog.OrderEdit,
                PermissionCatalog.OrderCancel,
                PermissionCatalog.OrderView,
                PermissionCatalog.OrderViewOwn,
                PermissionCatalog.OrderItemAdd,
                PermissionCatalog.OrderItemRemove
            },
            [WarehouseOperator] = new[]
            {
                PermissionCatalog.ProductView,
                PermissionCatalog.WarehouseView,
                PermissionCatalog.InventoryView,
                PermissionCatalog.InventoryAdjust,
                PermissionCatalog.OrderView,
                PermissionCatalog.OrderComplete
            },
            [Manager] = new[]
            {
                PermissionCatalog.CategoryView,
                PermissionCatalog.ProductView,
                PermissionCatalog.WarehouseView,
                PermissionCatalog.InventoryView,
                PermissionCatalog.OrderView,
                PermissionCatalog.OperatorView,
                PermissionCatalog.ReportViewOrders,
                PermissionCatalog.ReportViewInventory,
                PermissionCatalog.ReportViewTransactions,
                PermissionCatalog.ReportViewWarehouseSummary
            }
        };

    public static IReadOnlyList<string> DefaultPermissions(string roleName) =>
        Permissions.TryGetValue(roleName, out var permissions) ? permissions : new List<string>();
}