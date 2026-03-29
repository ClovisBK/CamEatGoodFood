using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentSupportOnRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentCount",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RecipeComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "100, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ParentCommentId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isEdited = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeComments_RecipeComments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "RecipeComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeComments_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fafac4c7-7e74-4808-9723-7ac814dd3c02", new DateTime(2026, 3, 11, 10, 18, 58, 95, DateTimeKind.Utc).AddTicks(7264), "AQAAAAIAAYagAAAAEE3QNL1NVCWfEle3STasvspsO7p/exSYkwjOtW2sVcgeH7U5tcnYw74k2XTX/g6npQ==", "9d1ce0eb-7cb3-42b0-806d-aa66bf583e96" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComments_ParentCommentId",
                table: "RecipeComments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComments_RecipeId_ParentCommentId_CreatedAt",
                table: "RecipeComments",
                columns: new[] { "RecipeId", "ParentCommentId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComments_UserId",
                table: "RecipeComments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeComments");

            migrationBuilder.DropColumn(
                name: "CommentCount",
                table: "Recipes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4f5011cf-b337-41ea-b4a5-4dd84c7384ba", new DateTime(2026, 3, 11, 8, 24, 13, 290, DateTimeKind.Utc).AddTicks(3562), "AQAAAAIAAYagAAAAEAPuIHheD+kSl3GUKl7uoWld3ZhmCMgNMYf7k8XLpH0/4CTNITjUsPiZ4UMDKk/L6A==", "339d2b7f-78b0-431c-8d19-25d73c6d8cda" });
        }
    }
}
