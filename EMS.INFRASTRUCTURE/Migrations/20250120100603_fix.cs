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
            migrationBuilder.DropIndex(
                name: "IX_Tasks_AddressId",
                table: "Tasks");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2d12e2c-ea38-47bb-9c08-a0217141edd0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df68b013-a32e-4fa6-b8fe-abd0f65c842b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "979ed47b-f070-41dc-8ac4-c7e29c5ff055", null, "User", "USER" },
                    { "b2934cb0-93cc-4cf8-a908-c98a2b9fa594", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AddressId",
                table: "Tasks",
                column: "AddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_AddressId",
                table: "Tasks");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "979ed47b-f070-41dc-8ac4-c7e29c5ff055");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2934cb0-93cc-4cf8-a908-c98a2b9fa594");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b2d12e2c-ea38-47bb-9c08-a0217141edd0", null, "Admin", "ADMIN" },
                    { "df68b013-a32e-4fa6-b8fe-abd0f65c842b", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AddressId",
                table: "Tasks",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");
        }
    }
}
