using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class madeestimateMinutesNullInInstructions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EstimatedMinutes",
                table: "RecipeInstructions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "491ac417-e8d8-4a81-80f5-d9b72f28989b", new DateTime(2026, 3, 27, 10, 32, 59, 322, DateTimeKind.Utc).AddTicks(2603), "AQAAAAIAAYagAAAAEGnnNiogRTshCT9PSP7U3LBCGb1DesMfj9HLVotF8FmsvtBe9230a9d8wU1OR21iCg==", "c357cd94-fc71-4e91-aa08-53314a0c8745" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EstimatedMinutes",
                table: "RecipeInstructions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fafac4c7-7e74-4808-9723-7ac814dd3c02", new DateTime(2026, 3, 11, 10, 18, 58, 95, DateTimeKind.Utc).AddTicks(7264), "AQAAAAIAAYagAAAAEE3QNL1NVCWfEle3STasvspsO7p/exSYkwjOtW2sVcgeH7U5tcnYw74k2XTX/g6npQ==", "9d1ce0eb-7cb3-42b0-806d-aa66bf583e96" });
        }
    }
}
