using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class AddedFieldsInApplicationUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "ProfilePictureUrl");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "Bio", "ConcurrencyStamp", "DateOfBirth", "FirstName", "Gender", "JoinedDate", "LastName", "Location", "PasswordHash", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "I am a certified chef that works with tasty foods", "500f986b-29a5-46c8-8734-cdd0b07be352", new DateTime(1992, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wa Kebeh", "Male", new DateTime(2026, 3, 3, 13, 55, 21, 316, DateTimeKind.Utc).AddTicks(2770), "Mbong", "Yaounde", "AQAAAAIAAYagAAAAEBa5KncOUzNVK1IkmnMznvBQIJsRB+1THiwIDjoU1wpyFHdf3/g713n31nwYQRTJ2g==", "myphoto.jpg", "9a062375-59c8-4eae-93d9-11fa90a37a99" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JoinedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                newName: "FullName");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100",
                columns: new[] { "ConcurrencyStamp", "FullName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "39e2193d-7579-44a8-9de2-009bba35585c", "Wa Kebeh Mbong", "AQAAAAIAAYagAAAAEMOiDUY+PxfcQ91KxZj0Vpy4KbqWW3dTK7zv8itXsS2dAdg/l67YCVobWh0wGasb3Q==", "e57576d9-cc53-4d04-8097-f68e9488f7cd" });
        }
    }
}
