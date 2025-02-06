using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class fixadddescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ce55e0d-b3b7-4d80-a22b-fd9cf805ace1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf4e7a0d-4e5a-4506-ad78-f9701c3880ba");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Locals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13581d36-07f7-4af9-96fc-fac9af62c2fd", null, "User", "USER" },
                    { "f33794e5-b162-406e-afcf-dc5242219639", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13581d36-07f7-4af9-96fc-fac9af62c2fd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f33794e5-b162-406e-afcf-dc5242219639");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Locals");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2ce55e0d-b3b7-4d80-a22b-fd9cf805ace1", null, "Admin", "ADMIN" },
                    { "cf4e7a0d-4e5a-4506-ad78-f9701c3880ba", null, "User", "USER" }
                });
        }
    }
}
