using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class addRecipeLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RecipeLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeLikes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "73f324dd-7a70-4e55-8f76-2cba6d69f48b", new DateTime(2026, 3, 11, 6, 59, 7, 593, DateTimeKind.Utc).AddTicks(4581), "AQAAAAIAAYagAAAAEIU3rt1kphQwFexdH5sKmHh9TNHkQ6cpYH3N625IwqRFDPa3pw7dyQHWiLXs8jrb3Q==", "4fc8a98d-622d-4727-bbb8-336fe06bbc55" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeLikes_RecipeId_UserId",
                table: "RecipeLikes",
                columns: new[] { "RecipeId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeLikes_UserId",
                table: "RecipeLikes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeLikes");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Recipes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fc89f5e4-fb61-4bcf-abfa-bc6548b275da", new DateTime(2026, 3, 4, 21, 1, 6, 93, DateTimeKind.Utc).AddTicks(3625), "AQAAAAIAAYagAAAAEAWa0Ik5LAzKW40falUJRl1fd4USeG1n1s/VBjsLfmtuIKUIaOoA/xkiSL0vejbg6Q==", "9c25a0a6-214d-4015-8285-e1e724e8e21a" });
        }
    }
}
