using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseOperators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseOperators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseOperators_Users_OperatorUserId",
                        column: x => x.OperatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseOperators_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.CheckConstraint("CK_Inventory_Quantity_NonNegative", "[Quantity] >= 0");
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    QuantityChange = table.Column<int>(type: "int", nullable: false),
                    PreviousQuantity = table.Column<int>(type: "int", nullable: false),
                    NewQuantity = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Electronic devices and accessories", true, "Electronics" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Food and household goods", true, "Groceries" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Office furniture and supplies", true, "Office Supplies" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DisplayName", "Email", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System Admin", "admin@inventory.local", true, "100000.MBzYxHzXa5ldDEWJ/l9oQg==.MP5l6mZmU1DpWNWDFJJoughwJ83sLGWVEN1DC+t9I5M=", "Admin", "admin" },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin2", "admin2@inventory.local", true, "100000.6u4ePiGDvOinOE45tjhHsA==.f4z3B7MINBuJu9tcM17xDV9J5rwCyUtaAcv93OBJtAA=", "Admin", "admin2" },
                    { new Guid("40000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ahmed Hassan", "ahmed@inventory.local", true, "100000.Bs0wWo3rhMJ4dh2IGmp8BQ==.coVOKfh2h6uNEcjuP1uqkSxBlfZ1HV5/nSa03XCJhcM=", "WarehouseOperator", "ahmed" },
                    { new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sara Mohamed", "sara@inventory.local", true, "100000.IUNFcOuRzyrW2TR497uubA==.6gQmot5WpeY3qPVa+71QKNJsAf8IUhWpOGn5Ri7YHiY=", "WarehouseOperator", "sara" },
                    { new Guid("40000000-0000-0000-0000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mohamed Ali", "mohamed@inventory.local", true, "100000.j3jvp6L3MnN649wsRRMT6A==.X+FdMk0qGX1xq4tGgg3PcoQGeW3ddONzDvFCG0vC0o0=", "WarehouseOperator", "mohamed" },
                    { new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", "manager@inventory.local", true, "100000.k19uFWjMCAzN+mGFdto86w==.z7SieNjkQ+HoYMlIfG815EZr7r54bCc/jF8FHWHc5Do=", "Manager", "manager" }
                });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Downtown, Cairo", "Main Warehouse" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Alexandria", "Alexandria Warehouse" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Nasr City, Cairo", "Cairo Warehouse" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "iPhone 15" },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Samsung 55 TV" },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Dell XPS 13 Laptop" },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Bluetooth Headphones" },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Smart Watch" },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Basmati Rice 5kg" },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Canned Beans" },
                    { new Guid("30000000-0000-0000-0000-000000000008"), new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Olive Oil" },
                    { new Guid("30000000-0000-0000-0000-000000000009"), new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Fresh Bread" },
                    { new Guid("30000000-0000-0000-0000-000000000010"), new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Almonds" },
                    { new Guid("30000000-0000-0000-0000-000000000011"), new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Office Chair" },
                    { new Guid("30000000-0000-0000-0000-000000000012"), new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Desk Lamp" },
                    { new Guid("30000000-0000-0000-0000-000000000013"), new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Notebook Set" },
                    { new Guid("30000000-0000-0000-0000-000000000014"), new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Pen Set" },
                    { new Guid("30000000-0000-0000-0000-000000000015"), new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Stapler" },
                    { new Guid("30000000-0000-0000-0000-000000000016"), new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "T-Shirt" },
                    { new Guid("30000000-0000-0000-0000-000000000017"), new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Jeans" },
                    { new Guid("30000000-0000-0000-0000-000000000018"), new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Jacket" },
                    { new Guid("30000000-0000-0000-0000-000000000019"), new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Coffee Maker" },
                    { new Guid("30000000-0000-0000-0000-000000000020"), new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Blender" }
                });

            migrationBuilder.InsertData(
                table: "WarehouseOperators",
                columns: new[] { "Id", "OperatorUserId", "WarehouseId" },
                values: new object[,]
                {
                    { new Guid("60000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("60000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000005"), new Guid("20000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.InsertData(
                table: "Inventories",
                columns: new[] { "Id", "ProductId", "Quantity", "UpdatedAt", "WarehouseId" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000001"), 50, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("30000000-0000-0000-0000-000000000002"), 25, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("30000000-0000-0000-0000-000000000003"), 12, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("30000000-0000-0000-0000-000000000004"), 45, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("30000000-0000-0000-0000-000000000005"), 18, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000006"), new Guid("30000000-0000-0000-0000-000000000006"), 55, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000007"), new Guid("30000000-0000-0000-0000-000000000007"), 30, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000008"), new Guid("30000000-0000-0000-0000-000000000008"), 20, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000009"), new Guid("30000000-0000-0000-0000-000000000009"), 50, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000010"), new Guid("30000000-0000-0000-0000-000000000010"), 15, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000011"), new Guid("30000000-0000-0000-0000-000000000011"), 50, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000012"), new Guid("30000000-0000-0000-0000-000000000012"), 15, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000013"), new Guid("30000000-0000-0000-0000-000000000013"), 11, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000014"), new Guid("30000000-0000-0000-0000-000000000014"), 55, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000015"), new Guid("30000000-0000-0000-0000-000000000015"), 22, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000016"), new Guid("30000000-0000-0000-0000-000000000016"), 47, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000017"), new Guid("30000000-0000-0000-0000-000000000017"), 28, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000018"), new Guid("30000000-0000-0000-0000-000000000018"), 22, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000019"), new Guid("30000000-0000-0000-0000-000000000019"), 47, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000020"), new Guid("30000000-0000-0000-0000-000000000020"), 32, new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "InventoryTransactions",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "InventoryId", "NewQuantity", "PreviousQuantity", "ProductId", "QuantityChange", "Reason", "Type", "WarehouseId" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 50, 0, new Guid("30000000-0000-0000-0000-000000000001"), 50, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 25, 0, new Guid("30000000-0000-0000-0000-000000000002"), 25, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 12, 0, new Guid("30000000-0000-0000-0000-000000000003"), 12, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 45, 0, new Guid("30000000-0000-0000-0000-000000000004"), 45, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 18, 0, new Guid("30000000-0000-0000-0000-000000000005"), 18, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 55, 0, new Guid("30000000-0000-0000-0000-000000000006"), 55, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000007"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 30, 0, new Guid("30000000-0000-0000-0000-000000000007"), 30, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000008"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 20, 0, new Guid("30000000-0000-0000-0000-000000000008"), 20, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000009"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 50, 0, new Guid("30000000-0000-0000-0000-000000000009"), 50, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000010"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 30, 0, new Guid("30000000-0000-0000-0000-000000000010"), 30, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000011"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 50, 0, new Guid("30000000-0000-0000-0000-000000000011"), 50, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000012"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 15, 0, new Guid("30000000-0000-0000-0000-000000000012"), 15, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000013"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 11, 0, new Guid("30000000-0000-0000-0000-000000000013"), 11, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000014"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 55, 0, new Guid("30000000-0000-0000-0000-000000000014"), 55, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000015"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 22, 0, new Guid("30000000-0000-0000-0000-000000000015"), 22, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000016"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 47, 0, new Guid("30000000-0000-0000-0000-000000000016"), 47, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000017"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 28, 0, new Guid("30000000-0000-0000-0000-000000000017"), 28, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000018"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 22, 0, new Guid("30000000-0000-0000-0000-000000000018"), 22, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000019"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 47, 0, new Guid("30000000-0000-0000-0000-000000000019"), 47, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000020"), new DateTime(2026, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001"), null, 32, 0, new Guid("30000000-0000-0000-0000-000000000020"), 32, "Initial stock on setup", 1, new Guid("20000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId_WarehouseId",
                table: "Inventories",
                columns: new[] { "ProductId", "WarehouseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_WarehouseId",
                table: "Inventories",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_CreatedAt",
                table: "InventoryTransactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_CreatedByUserId",
                table: "InventoryTransactions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_InventoryId",
                table: "InventoryTransactions",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_ProductId",
                table: "InventoryTransactions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_WarehouseId",
                table: "InventoryTransactions",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseOperators_OperatorUserId",
                table: "WarehouseOperators",
                column: "OperatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseOperators_WarehouseId_OperatorUserId",
                table: "WarehouseOperators",
                columns: new[] { "WarehouseId", "OperatorUserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryTransactions");

            migrationBuilder.DropTable(
                name: "WarehouseOperators");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
