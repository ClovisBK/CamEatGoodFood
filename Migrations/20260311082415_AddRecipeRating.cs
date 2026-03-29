using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Recipes",
                type: "float(3)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RatingCount",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RecipeRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "100, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeRatings_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4f5011cf-b337-41ea-b4a5-4dd84c7384ba", new DateTime(2026, 3, 11, 8, 24, 13, 290, DateTimeKind.Utc).AddTicks(3562), "AQAAAAIAAYagAAAAEAPuIHheD+kSl3GUKl7uoWld3ZhmCMgNMYf7k8XLpH0/4CTNITjUsPiZ4UMDKk/L6A==", "339d2b7f-78b0-431c-8d19-25d73c6d8cda" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeRatings_RecipeId_UserId",
                table: "RecipeRatings",
                columns: new[] { "RecipeId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeRatings_UserId",
                table: "RecipeRatings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "RatingCount",
                table: "Recipes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "73f324dd-7a70-4e55-8f76-2cba6d69f48b", new DateTime(2026, 3, 11, 6, 59, 7, 593, DateTimeKind.Utc).AddTicks(4581), "AQAAAAIAAYagAAAAEIU3rt1kphQwFexdH5sKmHh9TNHkQ6cpYH3N625IwqRFDPa3pw7dyQHWiLXs8jrb3Q==", "4fc8a98d-622d-4727-bbb8-336fe06bbc55" });
        }
    }
}
