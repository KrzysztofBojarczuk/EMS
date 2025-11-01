using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class vehiclechangedproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d8426044-bca1-4bf5-8e00-ebf3daa994ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f417c072-42c6-44e4-95f4-251deef387d6");

            migrationBuilder.RenameColumn(
                name: "LastServiceDate",
                table: "Vehicles",
                newName: "DateOfProduction");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "29afe4aa-1105-4d40-9735-0c42254a517f", null, "User", "USER" },
                    { "fb2df54b-a63c-4871-9aac-0ff43dfbab55", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29afe4aa-1105-4d40-9735-0c42254a517f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb2df54b-a63c-4871-9aac-0ff43dfbab55");

            migrationBuilder.RenameColumn(
                name: "DateOfProduction",
                table: "Vehicles",
                newName: "LastServiceDate");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d8426044-bca1-4bf5-8e00-ebf3daa994ed", null, "User", "USER" },
                    { "f417c072-42c6-44e4-95f4-251deef387d6", null, "Admin", "ADMIN" }
                });
        }
    }
}
