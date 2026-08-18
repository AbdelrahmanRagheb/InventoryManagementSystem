using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.Application.Authorization;

public static class PermissionCatalog
{
    public const string UserCreate = "User.Create";
    public const string UserEdit = "User.Edit";
    public const string UserDeactivate = "User.Deactivate";
    public const string UserAssignRole = "User.AssignRole";
    public const string UserManagePermissions = "User.ManagePermissions";

    public const string OperatorView = "Operator.View";

    public const string ProductCreate = "Product.Create";
    public const string ProductEdit = "Product.Edit";
    public const string ProductView = "Product.View";
    public const string ProductDeactivate = "Product.Deactivate";
    public const string ProductActivate = "Product.Activate";

    public const string CategoryCreate = "Category.Create";
    public const string CategoryEdit = "Category.Edit";
    public const string CategoryView = "Category.View";
    public const string CategoryDeactivate = "Category.Deactivate";
    public const string CategoryActivate = "Category.Activate";
    public const string CategoryDelete = "Category.Delete";

    public const string WarehouseCreate = "Warehouse.Create";
    public const string WarehouseEdit = "Warehouse.Edit";
    public const string WarehouseView = "Warehouse.View";
    public const string WarehouseDeactivate = "Warehouse.Deactivate";
    public const string WarehouseActivate = "Warehouse.Activate";

    public const string InventoryView = "Inventory.View";
    public const string InventoryAdjust = "Inventory.Adjust";

    public const string OrderCreate = "Order.Create";
    public const string OrderEdit = "Order.Edit";
    public const string OrderCancel = "Order.Cancel";
    public const string OrderComplete = "Order.Complete";
    public const string OrderView = "Order.View";
    public const string OrderViewOwn = "Order.ViewOwn";

    public const string OrderItemAdd = "OrderItem.Add";
    public const string OrderItemRemove = "OrderItem.Remove";

    public const string ReportViewOrders = "Report.ViewOrders";
    public const string ReportViewInventory = "Report.ViewInventory";
    public const string ReportViewTransactions = "Report.ViewTransactions";
    public const string ReportViewWarehouseSummary = "Report.ViewWarehouseSummary";

    public static readonly IReadOnlyDictionary<string, Guid> Ids = new Dictionary<string, Guid>
    {
        [UserCreate] = new("60000000-0000-0000-0000-000000000001"),
        [UserEdit] = new("60000000-0000-0000-0000-000000000002"),
        [UserDeactivate] = new("60000000-0000-0000-0000-000000000003"),
        [UserAssignRole] = new("60000000-0000-0000-0000-000000000004"),
        [UserManagePermissions] = new("60000000-0000-0000-0000-000000000005"),
        [ProductCreate] = new("60000000-0000-0000-0000-000000000006"),
        [ProductEdit] = new("60000000-0000-0000-0000-000000000007"),
        [ProductView] = new("60000000-0000-0000-0000-000000000008"),
        [ProductDeactivate] = new("60000000-0000-0000-0000-000000000009"),
        [ProductActivate] = new("60000000-0000-0000-0000-000000000010"),
        [CategoryCreate] = new("60000000-0000-0000-0000-000000000011"),
        [CategoryEdit] = new("60000000-0000-0000-0000-000000000012"),
        [CategoryView] = new("60000000-0000-0000-0000-000000000013"),
        [CategoryDeactivate] = new("60000000-0000-0000-0000-000000000014"),
        [CategoryActivate] = new("60000000-0000-0000-0000-000000000015"),
        [CategoryDelete] = new("60000000-0000-0000-0000-000000000016"),
        [WarehouseCreate] = new("60000000-0000-0000-0000-000000000017"),
        [WarehouseEdit] = new("60000000-0000-0000-0000-000000000018"),
        [WarehouseView] = new("60000000-0000-0000-0000-000000000019"),
        [WarehouseDeactivate] = new("60000000-0000-0000-0000-000000000020"),
        [WarehouseActivate] = new("60000000-0000-0000-0000-000000000021"),
        [InventoryView] = new("60000000-0000-0000-0000-000000000022"),
        [InventoryAdjust] = new("60000000-0000-0000-0000-000000000023"),
        [OrderCreate] = new("60000000-0000-0000-0000-000000000024"),
        [OrderEdit] = new("60000000-0000-0000-0000-000000000025"),
        [OrderCancel] = new("60000000-0000-0000-0000-000000000026"),
        [OrderComplete] = new("60000000-0000-0000-0000-000000000027"),
        [OrderView] = new("60000000-0000-0000-0000-000000000028"),
        [OrderViewOwn] = new("60000000-0000-0000-0000-000000000029"),
        [OrderItemAdd] = new("60000000-0000-0000-0000-000000000030"),
        [OrderItemRemove] = new("60000000-0000-0000-0000-000000000031"),
        [ReportViewOrders] = new("60000000-0000-0000-0000-000000000032"),
        [ReportViewInventory] = new("60000000-0000-0000-0000-000000000033"),
        [ReportViewTransactions] = new("60000000-0000-0000-0000-000000000034"),
        [ReportViewWarehouseSummary] = new("60000000-0000-0000-0000-000000000035"),
        [OperatorView] = new("60000000-0000-0000-0000-000000000036")
    };

    public static readonly IReadOnlyDictionary<string, string> Descriptions = new Dictionary<string, string>
    {
        [UserCreate] = "Create new user accounts",
        [UserEdit] = "Edit user profile fields",
        [UserDeactivate] = "Deactivate (disable) user accounts",
        [UserAssignRole] = "Assign a role to a user (replaces their permissions with the role defaults)",
        [UserManagePermissions] = "Grant and revoke user permissions",
        [ProductCreate] = "Create products",
        [ProductEdit] = "Edit products",
        [ProductView] = "View products",
        [ProductDeactivate] = "Deactivate products",
        [ProductActivate] = "Activate products",
        [CategoryCreate] = "Create categories",
        [CategoryEdit] = "Edit categories",
        [CategoryView] = "View categories",
        [CategoryDeactivate] = "Deactivate categories",
        [CategoryActivate] = "Activate categories",
        [CategoryDelete] = "Hard-delete categories",
        [WarehouseCreate] = "Create warehouses",
        [WarehouseEdit] = "Edit warehouses (warehouse-scoped)",
        [WarehouseView] = "View warehouses",
        [WarehouseDeactivate] = "Deactivate warehouses (warehouse-scoped)",
        [WarehouseActivate] = "Activate warehouses (warehouse-scoped)",
        [InventoryView] = "View inventory and stock movement (transactions)",
        [InventoryAdjust] = "Adjust stock counts (warehouse-scoped)",
        [OrderCreate] = "Create orders",
        [OrderEdit] = "Edit orders",
        [OrderCancel] = "Cancel orders",
        [OrderComplete] = "Complete (fulfill) orders (warehouse-scoped)",
        [OrderView] = "View all orders",
        [OrderViewOwn] = "View own orders only",
        [OrderItemAdd] = "Add line items to orders",
        [OrderItemRemove] = "Remove line items from orders",
        [ReportViewOrders] = "View order reports",
        [ReportViewInventory] = "View inventory reports",
        [ReportViewTransactions] = "View transaction reports",
        [ReportViewWarehouseSummary] = "View warehouse summary reports",
        [OperatorView] = "View operator assignments"
    };

    public static readonly IReadOnlyList<string> All = Ids.Keys.ToList();

    public static readonly IReadOnlySet<string> WarehouseScoped = new HashSet<string>
    {
        WarehouseEdit,
        WarehouseDeactivate,
        WarehouseActivate,
        InventoryAdjust,
        OrderComplete
    };

    public static Guid Id(string permission) => Ids[permission];

    public static bool IsWarehouseScoped(string permission) => WarehouseScoped.Contains(permission);

    public static bool Exists(string permission) => Ids.ContainsKey(permission);
}