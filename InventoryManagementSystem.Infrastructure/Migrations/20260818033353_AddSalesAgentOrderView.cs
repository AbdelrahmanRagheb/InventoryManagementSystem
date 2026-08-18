using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesAgentOrderView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserPermissions",
                columns: new[] { "Id", "PermissionId", "ResourceId", "UserId" },
                values: new object[] { new Guid("80000000-0000-0000-0000-000000000035"), new Guid("60000000-0000-0000-0000-000000000028"), null, new Guid("40000000-0000-0000-0000-000000000007") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserPermissions",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000035"));
        }
    }
}
