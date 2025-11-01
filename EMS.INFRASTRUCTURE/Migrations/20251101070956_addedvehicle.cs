using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class addedvehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleEntity_AspNetUsers_AppUserId",
                table: "VehicleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleEntity_Tasks_TaskId",
                table: "VehicleEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleEntity",
                table: "VehicleEntity");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05810c10-9574-4f39-85ca-437d24fd5ab5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f2fd7e0-f343-4278-8512-371a4b9d607c");

            migrationBuilder.RenameTable(
                name: "VehicleEntity",
                newName: "Vehicles");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleEntity_TaskId",
                table: "Vehicles",
                newName: "IX_Vehicles_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleEntity_AppUserId",
                table: "Vehicles",
                newName: "IX_Vehicles_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d8426044-bca1-4bf5-8e00-ebf3daa994ed", null, "User", "USER" },
                    { "f417c072-42c6-44e4-95f4-251deef387d6", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AspNetUsers_AppUserId",
                table: "Vehicles",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Tasks_TaskId",
                table: "Vehicles",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_AppUserId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Tasks_TaskId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d8426044-bca1-4bf5-8e00-ebf3daa994ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f417c072-42c6-44e4-95f4-251deef387d6");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "VehicleEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_TaskId",
                table: "VehicleEntity",
                newName: "IX_VehicleEntity_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_AppUserId",
                table: "VehicleEntity",
                newName: "IX_VehicleEntity_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleEntity",
                table: "VehicleEntity",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "05810c10-9574-4f39-85ca-437d24fd5ab5", null, "Admin", "ADMIN" },
                    { "5f2fd7e0-f343-4278-8512-371a4b9d607c", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleEntity_AspNetUsers_AppUserId",
                table: "VehicleEntity",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleEntity_Tasks_TaskId",
                table: "VehicleEntity",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
