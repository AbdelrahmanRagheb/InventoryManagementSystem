using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "ResourceId",
                table: "Warehouses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("60000000-0000-0000-0000-000000000001"), "Create new user accounts", "User.Create" },
                    { new Guid("60000000-0000-0000-0000-000000000002"), "Edit user profile fields", "User.Edit" },
                    { new Guid("60000000-0000-0000-0000-000000000003"), "Deactivate (disable) user accounts", "User.Deactivate" },
                    { new Guid("60000000-0000-0000-0000-000000000004"), "Assign a role to a user (replaces their permissions with the role defaults)", "User.AssignRole" },
                    { new Guid("60000000-0000-0000-0000-000000000005"), "Grant and revoke user permissions", "User.ManagePermissions" },
                    { new Guid("60000000-0000-0000-0000-000000000006"), "Create products", "Product.Create" },
                    { new Guid("60000000-0000-0000-0000-000000000007"), "Edit products", "Product.Edit" },
                    { new Guid("60000000-0000-0000-0000-000000000008"), "View products", "Product.View" },
                    { new Guid("60000000-0000-0000-0000-000000000009"), "Deactivate products", "Product.Deactivate" },
                    { new Guid("60000000-0000-0000-0000-000000000010"), "Activate products", "Product.Activate" },
                    { new Guid("60000000-0000-0000-0000-000000000011"), "Create categories", "Category.Create" },
                    { new Guid("60000000-0000-0000-0000-000000000012"), "Edit categories", "Category.Edit" },
                    { new Guid("60000000-0000-0000-0000-000000000013"), "View categories", "Category.View" },
                    { new Guid("60000000-0000-0000-0000-000000000014"), "Deactivate categories", "Category.Deactivate" },
                    { new Guid("60000000-0000-0000-0000-000000000015"), "Activate categories", "Category.Activate" },
                    { new Guid("60000000-0000-0000-0000-000000000016"), "Hard-delete categories", "Category.Delete" },
                    { new Guid("60000000-0000-0000-0000-000000000017"), "Create warehouses", "Warehouse.Create" },
                    { new Guid("60000000-0000-0000-0000-000000000018"), "Edit warehouses (warehouse-scoped)", "Warehouse.Edit" },
                    { new Guid("60000000-0000-0000-0000-000000000019"), "View warehouses", "Warehouse.View" },
                    { new Guid("60000000-0000-0000-0000-000000000020"), "Deactivate warehouses (warehouse-scoped)", "Warehouse.Deactivate" },
                    { new Guid("60000000-0000-0000-0000-000000000021"), "Activate warehouses (warehouse-scoped)", "Warehouse.Activate" },
                    { new Guid("60000000-0000-0000-0000-000000000022"), "View inventory and stock movement (transactions)", "Inventory.View" },
                    { new Guid("60000000-0000-0000-0000-000000000023"), "Adjust stock counts (warehouse-scoped)", "Inventory.Adjust" },
                    { new Guid("60000000-0000-0000-0000-000000000024"), "Create orders", "Order.Create" },
                    { new Guid("60000000-0000-0000-0000-000000000025"), "Edit orders", "Order.Edit" },
                    { new Guid("60000000-0000-0000-0000-000000000026"), "Cancel orders", "Order.Cancel" },
                    { new Guid("60000000-0000-0000-0000-000000000027"), "Complete (fulfill) orders (warehouse-scoped)", "Order.Complete" },
                    { new Guid("60000000-0000-0000-0000-000000000028"), "View all orders", "Order.View" },
                    { new Guid("60000000-0000-0000-0000-000000000029"), "View own orders only", "Order.ViewOwn" },
                    { new Guid("60000000-0000-0000-0000-000000000030"), "Add line items to orders", "OrderItem.Add" },
                    { new Guid("60000000-0000-0000-0000-000000000031"), "Remove line items from orders", "OrderItem.Remove" },
                    { new Guid("60000000-0000-0000-0000-000000000032"), "View order reports", "Report.ViewOrders" },
                    { new Guid("60000000-0000-0000-0000-000000000033"), "View inventory reports", "Report.ViewInventory" },
                    { new Guid("60000000-0000-0000-0000-000000000034"), "View transaction reports", "Report.ViewTransactions" },
                    { new Guid("60000000-0000-0000-0000-000000000035"), "View warehouse summary reports", "Report.ViewWarehouseSummary" }
                });

            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000001"), "Main Warehouse", "Warehouse" },
                    { new Guid("70000000-0000-0000-0000-000000000002"), "Alexandria Warehouse", "Warehouse" },
                    { new Guid("70000000-0000-0000-0000-000000000003"), "Cairo Warehouse", "Warehouse" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), "Admin" },
                    { new Guid("50000000-0000-0000-0000-000000000002"), "Manager" },
                    { new Guid("50000000-0000-0000-0000-000000000003"), "WarehouseOperator" },
                    { new Guid("50000000-0000-0000-0000-000000000004"), "SalesAgent" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"),
                column: "RoleId",
                value: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"),
                column: "RoleId",
                value: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000003"),
                column: "RoleId",
                value: new Guid("50000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000004"),
                column: "RoleId",
                value: new Guid("50000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000005"),
                column: "RoleId",
                value: new Guid("50000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000006"),
                column: "RoleId",
                value: new Guid("50000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000007"),
                column: "RoleId",
                value: new Guid("50000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"),
                column: "ResourceId",
                value: new Guid("70000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"),
                column: "ResourceId",
                value: new Guid("70000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"),
                column: "ResourceId",
                value: new Guid("70000000-0000-0000-0000-000000000003"));

            migrationBuilder.InsertData(
                table: "UserPermissions",
                columns: new[] { "Id", "PermissionId", "ResourceId", "UserId" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000000"), new Guid("60000000-0000-0000-0000-000000000008"), null, new Guid("40000000-0000-0000-0000-000000000003") },
                    { new Guid("80000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000019"), null, new Guid("40000000-0000-0000-0000-000000000003") },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new Guid("60000000-0000-0000-0000-000000000022"), null, new Guid("40000000-0000-0000-0000-000000000003") },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new Guid("60000000-0000-0000-0000-000000000023"), null, new Guid("40000000-0000-0000-0000-000000000003") },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new Guid("60000000-0000-0000-0000-000000000028"), null, new Guid("40000000-0000-0000-0000-000000000003") },
                    { new Guid("80000000-0000-0000-0000-000000000005"), new Guid("60000000-0000-0000-0000-000000000027"), null, new Guid("40000000-0000-0000-0000-000000000003") },
                    { new Guid("80000000-0000-0000-0000-000000000006"), new Guid("60000000-0000-0000-0000-000000000008"), null, new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000007"), new Guid("60000000-0000-0000-0000-000000000019"), null, new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000008"), new Guid("60000000-0000-0000-0000-000000000022"), null, new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000009"), new Guid("60000000-0000-0000-0000-000000000023"), null, new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000010"), new Guid("60000000-0000-0000-0000-000000000028"), null, new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000011"), new Guid("60000000-0000-0000-0000-000000000027"), null, new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000012"), new Guid("60000000-0000-0000-0000-000000000008"), null, new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("80000000-0000-0000-0000-000000000013"), new Guid("60000000-0000-0000-0000-000000000019"), null, new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("80000000-0000-0000-0000-000000000014"), new Guid("60000000-0000-0000-0000-000000000022"), null, new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("80000000-0000-0000-0000-000000000015"), new Guid("60000000-0000-0000-0000-000000000023"), null, new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("80000000-0000-0000-0000-000000000016"), new Guid("60000000-0000-0000-0000-000000000028"), null, new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("80000000-0000-0000-0000-000000000017"), new Guid("60000000-0000-0000-0000-000000000027"), null, new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("80000000-0000-0000-0000-000000000018"), new Guid("60000000-0000-0000-0000-000000000013"), null, new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000019"), new Guid("60000000-0000-0000-0000-000000000008"), null, new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000020"), new Guid("60000000-0000-0000-0000-000000000019"), null, new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000021"), new Guid("60000000-0000-0000-0000-000000000022"), null, new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000022"), new Guid("60000000-0000-0000-0000-000000000028"), null, new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000023"), new Guid("60000000-0000-0000-0000-000000000032"), null, new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000024"), new Guid("60000000-0000-0000-0000-000000000033"), null, new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000025"), new Guid("60000000-0000-0000-0000-000000000034"), null, new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000026"), new Guid("60000000-0000-0000-0000-000000000035"), null, new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000027"), new Guid("60000000-0000-0000-0000-000000000008"), null, new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("80000000-0000-0000-0000-000000000028"), new Guid("60000000-0000-0000-0000-000000000024"), null, new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("80000000-0000-0000-0000-000000000029"), new Guid("60000000-0000-0000-0000-000000000025"), null, new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("80000000-0000-0000-0000-000000000030"), new Guid("60000000-0000-0000-0000-000000000026"), null, new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("80000000-0000-0000-0000-000000000031"), new Guid("60000000-0000-0000-0000-000000000029"), null, new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("80000000-0000-0000-0000-000000000032"), new Guid("60000000-0000-0000-0000-000000000030"), null, new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("80000000-0000-0000-0000-000000000033"), new Guid("60000000-0000-0000-0000-000000000031"), null, new Guid("40000000-0000-0000-0000-000000000007") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_ResourceId",
                table: "Warehouses",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_Type_Name",
                table: "Resources",
                columns: new[] { "Type", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_ResourceId",
                table: "UserPermissions",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId",
                table: "UserPermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId_PermissionId",
                table: "UserPermissions",
                columns: new[] { "UserId", "PermissionId" },
                unique: true,
                filter: "[ResourceId] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId_PermissionId_ResourceId",
                table: "UserPermissions",
                columns: new[] { "UserId", "PermissionId", "ResourceId" },
                unique: true,
                filter: "[ResourceId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Resources_ResourceId",
                table: "Warehouses",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Resources_ResourceId",
                table: "Warehouses");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_ResourceId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"),
                column: "Role",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"),
                column: "Role",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000003"),
                column: "Role",
                value: "WarehouseOperator");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000004"),
                column: "Role",
                value: "WarehouseOperator");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000005"),
                column: "Role",
                value: "WarehouseOperator");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000006"),
                column: "Role",
                value: "Manager");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000007"),
                column: "Role",
                value: "SalesAgent");
        }
    }
}
