using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class AddedUnitsOfIngredientMeaurementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5d76fa62-526f-4841-b56f-41e446c655ca", new DateTime(2026, 3, 4, 9, 42, 5, 722, DateTimeKind.Utc).AddTicks(7893), "AQAAAAIAAYagAAAAEIe7dbqOErkab1orwIA2klcBr7BiaApiKZI9PCtWuJXZ2Akzjp383R0VMm1sb1prYg==", "bca270df-117c-4541-ad38-407f48ce8aa1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "JoinedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2bb169a7-6bcb-4f05-a29c-4d0c36ff130f", new DateTime(2026, 3, 4, 9, 5, 14, 744, DateTimeKind.Utc).AddTicks(2926), "AQAAAAIAAYagAAAAEL4X3Zs+j71pr2U4zW8yWG2tW0O4T9r5PmEikTnrHDHco1mA6TuOAabCzVXuNbzpFQ==", "668286e1-3cb9-4c3b-99b1-c5499e84fb1c" });
        }
    }
}
