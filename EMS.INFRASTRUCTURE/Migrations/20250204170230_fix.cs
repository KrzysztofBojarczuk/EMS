using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10bfa849-2653-4ac1-ae73-4399fbe67399");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b090bfd-b035-45b8-9f74-43f5c1ab6610");

            migrationBuilder.RenameColumn(
                name: "LoclalNumber",
                table: "Locals",
                newName: "LocalNumber");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2ce55e0d-b3b7-4d80-a22b-fd9cf805ace1", null, "Admin", "ADMIN" },
                    { "cf4e7a0d-4e5a-4506-ad78-f9701c3880ba", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ce55e0d-b3b7-4d80-a22b-fd9cf805ace1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf4e7a0d-4e5a-4506-ad78-f9701c3880ba");

            migrationBuilder.RenameColumn(
                name: "LocalNumber",
                table: "Locals",
                newName: "LoclalNumber");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10bfa849-2653-4ac1-ae73-4399fbe67399", null, "User", "USER" },
                    { "3b090bfd-b035-45b8-9f74-43f5c1ab6610", null, "Admin", "ADMIN" }
                });
        }
    }
}
