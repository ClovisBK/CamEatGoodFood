using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class AddedRecipeCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipeCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeCategories", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2bb169a7-6bcb-4f05-a29c-4d0c36ff130f", new DateTime(2026, 3, 4, 9, 5, 14, 744, DateTimeKind.Utc).AddTicks(2926), "AQAAAAIAAYagAAAAEL4X3Zs+j71pr2U4zW8yWG2tW0O4T9r5PmEikTnrHDHco1mA6TuOAabCzVXuNbzpFQ==", "668286e1-3cb9-4c3b-99b1-c5499e84fb1c" });

            migrationBuilder.InsertData(
                table: "RecipeCategories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Traditional Cameroonian soups and delicacies", "Soups" },
                    { 2, "Complete meals for lunch and dinner", "Main Dishes" },
                    { 3, "Morning meals and small chops", "Breakfast" },
                    { 4, "Small bites and street food", "Snacks" },
                    { 5, "Drinks and smoothies", "Beverages" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeCategories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "500f986b-29a5-46c8-8734-cdd0b07be352", new DateTime(2026, 3, 3, 13, 55, 21, 316, DateTimeKind.Utc).AddTicks(2770), "AQAAAAIAAYagAAAAEBa5KncOUzNVK1IkmnMznvBQIJsRB+1THiwIDjoU1wpyFHdf3/g713n31nwYQRTJ2g==", "9a062375-59c8-4eae-93d9-11fa90a37a99" });
        }
    }
}
