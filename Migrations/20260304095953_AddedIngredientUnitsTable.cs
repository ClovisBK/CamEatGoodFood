using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class AddedIngredientUnitsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IngredientUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsBaseUnit = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientUnits", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d2da5e7f-e543-4de8-a56c-66302a939f1e", new DateTime(2026, 3, 4, 9, 59, 52, 211, DateTimeKind.Utc).AddTicks(9513), "AQAAAAIAAYagAAAAEHaMDC9SemwubEuFmkO9qD4puMr4d9cUd43e5vZoGgGLaAxCmEMQ+8i6xKLtcGF1qQ==", "8e679f41-e1b5-463b-b96f-a3c2c6bfdee4" });

            migrationBuilder.InsertData(
                table: "IngredientUnits",
                columns: new[] { "Id", "Abbreviation", "IsBaseUnit", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "g", true, "gram", "weight" },
                    { 2, "kg", false, "kilogram", "weight" },
                    { 3, "mg", false, "milligram", "weight" },
                    { 4, "ml", true, "milliliter", "volume" },
                    { 5, "L", false, "liter", "volume" },
                    { 6, "tbsp", false, "tablespoon", "volume" },
                    { 7, "tsp", false, "teaspoon", "volume" },
                    { 8, "cup", false, "cup", "volume" },
                    { 9, "pc", true, "piece", "count" },
                    { 10, "med", false, "medium", "count" },
                    { 11, "lg", false, "large", "count" },
                    { 12, "sm", false, "small", "count" },
                    { 13, "clove", false, "clove", "count" },
                    { 14, "hf", false, "handful", "count" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientUnits");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5d76fa62-526f-4841-b56f-41e446c655ca", new DateTime(2026, 3, 4, 9, 42, 5, 722, DateTimeKind.Utc).AddTicks(7893), "AQAAAAIAAYagAAAAEIe7dbqOErkab1orwIA2klcBr7BiaApiKZI9PCtWuJXZ2Akzjp383R0VMm1sb1prYg==", "bca270df-117c-4541-ad38-407f48ce8aa1" });
        }
    }
}
