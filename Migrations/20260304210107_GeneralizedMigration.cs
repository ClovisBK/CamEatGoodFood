using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class GeneralizedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultUnitId = table.Column<int>(type: "int", nullable: false),
                    CaloriesPer100g = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Proteins = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Carbohydrates = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Fats = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Fibers = table.Column<decimal>(type: "decimal(7,2)", nullable: true),
                    Sodium = table.Column<decimal>(type: "decimal(7,2)", nullable: true),
                    GramsPerUnit = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Density = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    IngredientUnitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_IngredientUnits_IngredientUnitId",
                        column: x => x.IngredientUnitId,
                        principalTable: "IngredientUnits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PrepTimeMinutes = table.Column<int>(type: "int", nullable: false),
                    CookTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    Servings = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionOfOrigin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recipes_RecipeCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "RecipeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_IngredientUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "IngredientUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeInstructions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    StepNumber = table.Column<int>(type: "int", nullable: false),
                    ActualInstruction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedMinutes = table.Column<int>(type: "int", nullable: false),
                    IsOptional = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeInstructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeInstructions_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Bio", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "Gender", "JoinedDate", "LastName", "Location", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "Phone", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureUrl", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "100", 0, "I am a certified chef that works with tasty foods", "fc89f5e4-fb61-4bcf-abfa-bc6548b275da", new DateTime(1992, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "wakebeh14@gmail.com", true, "Wa Kebeh", "Male", new DateTime(2026, 3, 4, 21, 1, 6, 93, DateTimeKind.Utc).AddTicks(3625), "Mbong", "Yaounde", false, null, "WAKEBEH14@GMAIL.COM", "ADMIN@SYSTEM.COM", "AQAAAAIAAYagAAAAEAWa0Ik5LAzKW40falUJRl1fd4USeG1n1s/VBjsLfmtuIKUIaOoA/xkiSL0vejbg6Q==", "676455676", null, false, "myphoto.jpg", "9c25a0a6-214d-4015-8285-e1e724e8e21a", false, "admin@system.com" });

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

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "CaloriesPer100g", "Carbohydrates", "DefaultUnitId", "Density", "Fats", "Fibers", "GramsPerUnit", "IngredientUnitId", "Name", "Proteins", "Sodium" },
                values: new object[,]
                {
                    { 101, 250.00m, 0.00m, 1, null, 20.00m, 0.00m, null, null, "Beef", 26.00m, 0.05m },
                    { 102, 165.00m, 0.00m, 1, null, 3.60m, 0.00m, null, null, "Chicken breast", 31.00m, 0.07m },
                    { 103, 209.00m, 0.00m, 1, null, 11.00m, 0.00m, null, null, "Chicken thighs", 26.00m, 0.08m },
                    { 104, 172.00m, 0.00m, 1, null, 6.00m, 0.00m, null, null, "Chicken drumsticks", 28.00m, 0.09m },
                    { 105, 143.00m, 0.00m, 1, null, 3.00m, 0.00m, null, null, "Goat meat", 27.00m, 0.08m },
                    { 106, 242.00m, 0.00m, 1, null, 14.00m, 0.00m, null, null, "Pork", 27.00m, 0.06m },
                    { 107, 321.00m, 0.00m, 1, null, 28.00m, 0.00m, null, null, "Pork ribs", 16.00m, 0.07m },
                    { 108, 135.00m, 0.00m, 1, null, 1.00m, 0.00m, null, null, "Turkey", 29.00m, 0.07m },
                    { 109, 337.00m, 0.00m, 1, null, 28.00m, 0.00m, null, null, "Duck", 19.00m, 0.07m },
                    { 110, 136.00m, 9.00m, 10, null, 0.10m, 1.70m, 110.00m, null, "Onion", 1.10m, 0.04m },        
                    { 111, 18.00m, 3.90m, 10, null, 0.20m, 1.20m, 123.00m, null, "Tomatoes", 0.90m, 0.05m },     
                    { 112, 149.00m, 33.00m, 13, null, 0.50m, 2.10m, 3.00m, null, "Garlic", 6.40m, 0.01m },      
                    { 113, 0.00m, 0.00m, 7, 1.20m, 0.00m, 0.00m, null, null, "Salt", 0.00m, 38.76m },              
                    { 114, 255.00m, 50.00m, 7, 0.50m, 3.00m, 25.00m, 2.00m, null, "Pepper", 10.00m, 0.02m }, 
                    { 115, 290.00m, 0.00m, 1, null, 19.00m, 0.00m, null, null, "Smoked beef", 28.00m, 1.20m },
                    { 116, 128.00m, 0.00m, 1, null, 2.70m, 0.00m, null, null, "Fresh fish (tilapia)", 26.00m, 0.06m },
                    { 117, 205.00m, 0.00m, 1, null, 14.00m, 0.00m, null, null, "Mackerel (maquereau)", 19.00m, 0.08m },
                    { 118, 162.00m, 0.00m, 1, null, 9.00m, 0.00m, null, null, "Catfish (poisson-chat)", 18.00m, 0.06m },
                    { 119, 150.00m, 0.00m, 1, null, 6.00m, 0.00m, null, null, "Croaker (corb)", 22.00m, 0.07m },
                    { 120, 175.00m, 0.00m, 1, null, 8.00m, 0.00m, null, null, "Barracuda", 24.00m, 0.08m },
                    { 121, 158.00m, 0.00m, 1, null, 8.00m, 0.00m, null, null, "Bonga fish (poisson bonga)", 21.00m, 0.09m },
                    { 122, 208.00m, 0.00m, 1, null, 11.00m, 0.00m, null, null, "Sardines", 25.00m, 0.05m },
                    { 123, 184.00m, 0.00m, 1, null, 6.00m, 0.00m, null, null, "Tuna", 30.00m, 0.05m },
                    { 124, 128.00m, 0.00m, 1, null, 2.00m, 0.00m, null, null, "Snapper (vivaneau)", 26.00m, 0.06m },
                    { 125, 190.00m, 0.00m, 1, null, 8.00m, 0.00m, null, null, "Smoked fish", 28.00m, 1.50m },
                    { 126, 280.00m, 0.00m, 1, null, 8.00m, 0.00m, null, null, "Dried fish", 50.00m, 2.00m },
                    { 127, 85.00m, 0.00m, 1, null, 0.50m, 0.00m, 10.00m, null, "Shrimp (fresh)", 20.00m, 0.30m },
                    { 128, 250.00m, 0.00m, 1, null, 5.00m, 0.00m, 2.00m, null, "Dried shrimp", 50.00m, 2.50m },
                    { 129, 87.00m, 0.00m, 1, null, 1.50m, 0.00m, null, null, "Crab", 18.00m, 0.30m },
                    { 130, 110.00m, 0.00m, 1, null, 2.00m, 0.00m, 5.00m, null, "Crayfish", 22.00m, 0.20m },
                    { 131, 45.00m, 5.00m, 1, null, 1.00m, 4.00m, null, null, "Ndolé leaves (bitter leaf)", 4.50m, 0.01m },
                    { 132, 50.00m, 6.00m, 1, null, 1.50m, 3.00m, null, null, "Eru leaves", 4.00m, 0.01m },
                    { 133, 33.00m, 7.00m, 1, null, 0.20m, 3.20m, 15.00m, null, "Okra (gombo)", 2.00m, 0.01m },
                    { 134, 23.00m, 3.00m, 1, null, 0.40m, 1.50m, null, null, "Waterleaf", 2.50m, 0.01m },
                    { 135, 42.00m, 5.00m, 1, null, 0.80m, 3.50m, null, null, "Huckleberry leaves (folong)", 4.00m, 0.01m },
                    { 136, 91.00m, 13.00m, 1, null, 2.00m, 5.00m, null, null, "Cassava leaves", 7.00m, 0.01m },
                    { 137, 32.00m, 5.00m, 1, null, 0.40m, 2.50m, null, null, "Pumpkin leaves", 3.00m, 0.01m },
                    { 138, 23.00m, 3.60m, 1, null, 0.40m, 2.20m, null, null, "Spinach", 2.90m, 0.08m },
                    { 139, 25.00m, 5.80m, 1, null, 0.10m, 2.50m, null, null, "Cabbage", 1.30m, 0.02m },
                    { 140, 15.00m, 2.90m, 1, null, 0.20m, 1.30m, null, null, "Lettuce", 1.40m, 0.03m },
                    { 141, 41.00m, 9.60m, 10, null, 0.20m, 2.80m, 60.00m, null, "Carrot", 0.90m, 0.07m },
                    { 142, 20.00m, 4.60m, 10, null, 0.20m, 1.70m, 120.00m, null, "Green bell pepper", 0.90m, 0.00m },
                    { 143, 31.00m, 6.00m, 10, null, 0.30m, 2.10m, 120.00m, null, "Red bell pepper", 1.00m, 0.00m },
                    { 144, 40.00m, 8.80m, 13, null, 0.40m, 1.50m, 10.00m, null, "Hot pepper (piment)", 1.90m, 0.01m },
                    { 145, 25.00m, 5.70m, 10, null, 0.20m, 3.00m, 80.00m, null, "African eggplant (aubergine)", 1.00m, 0.00m },
                    { 146, 17.00m, 3.10m, 10, null, 0.30m, 1.00m, 200.00m, null, "Zucchini", 1.20m, 0.01m },
                    { 147, 31.00m, 7.00m, 1, null, 0.20m, 3.40m, null, null, "Green beans", 1.80m, 0.01m },
                    { 148, 81.00m, 14.40m, 1, null, 0.40m, 5.00m, null, null, "Peas", 5.40m, 0.01m },
                    { 149, 86.00m, 19.00m, 10, null, 1.20m, 2.70m, 150.00m, null, "Corn (maize)", 3.20m, 0.02m },
                    { 150, 22.00m, 3.30m, 1, null, 0.30m, 1.00m, null, null, "Mushrooms", 3.10m, 0.01m },
                    { 151, 160.00m, 38.00m, 1, null, 0.30m, 1.80m, null, null, "Cassava (fresh)", 1.40m, 0.01m },
                    { 152, 150.00m, 36.00m, 1, null, 0.30m, 1.70m, null, null, "Cassava (frozen)", 1.30m, 0.01m },
                    { 153, 118.00m, 28.00m, 1, null, 0.20m, 4.10m, null, null, "Yam", 1.50m, 0.01m },
                    { 154, 86.00m, 20.00m, 10, null, 0.10m, 3.00m, 150.00m, null, "Sweet potato (orange)", 1.60m, 0.06m },
                    { 155, 90.00m, 21.00m, 10, null, 0.10m, 2.50m, 150.00m, null, "Sweet potato (white)", 1.60m, 0.06m },
                    { 156, 112.00m, 26.00m, 1, null, 0.20m, 2.50m, null, null, "Cocoyam (macabo)", 1.50m, 0.01m },
                    { 157, 112.00m, 26.00m, 1, null, 0.20m, 4.00m, null, null, "Taro (pomme de terre)", 1.50m, 0.01m },
                    { 158, 77.00m, 17.00m, 10, null, 0.10m, 2.20m, 150.00m, null, "Potato (Irish potato)", 2.00m, 0.01m },
                    { 159, 116.00m, 31.00m, 10, null, 0.40m, 2.30m, 200.00m, null, "Plantain (green/unripe)", 1.20m, 0.00m },
                    { 160, 122.00m, 32.00m, 10, null, 0.40m, 2.30m, 200.00m, null, "Plantain (ripe/yellow)", 1.30m, 0.00m },
                    { 161, 130.00m, 34.00m, 10, null, 0.40m, 2.00m, 200.00m, null, "Plantain (very ripe/black)", 1.30m, 0.00m },
                    { 162, 89.00m, 22.80m, 10, null, 0.30m, 2.60m, 120.00m, null, "Banana (dessert)", 1.10m, 0.00m },
                    { 163, 350.00m, 80.00m, 1, null, 1.00m, 2.00m, null, null, "Fufu flour (corn)", 3.00m, 0.01m },
                    { 164, 360.00m, 88.00m, 1, null, 0.50m, 2.00m, null, null, "Fufu flour (cassava)", 1.00m, 0.01m },
                    { 165, 350.00m, 85.00m, 1, null, 0.50m, 3.00m, null, null, "Garri (cassava granules)", 1.50m, 0.01m },
                    { 166, 60.00m, 15.00m, 10, null, 0.40m, 1.60m, 200.00m, null, "Mango", 0.80m, 0.00m },
                    { 167, 50.00m, 13.00m, 1, null, 0.10m, 1.40m, null, null, "Pineapple", 0.50m, 0.00m },
                    { 168, 43.00m, 11.00m, 1, null, 0.30m, 1.70m, null, null, "Papaya", 0.50m, 0.01m },
                    { 169, 160.00m, 8.50m, 10, null, 15.00m, 6.70m, 150.00m, null, "Avocado", 2.00m, 0.01m },
                    { 170, 47.00m, 12.00m, 10, null, 0.10m, 2.40m, 130.00m, null, "Orange", 0.90m, 0.00m },
                    { 171, 53.00m, 13.00m, 10, null, 0.30m, 1.80m, 80.00m, null, "Tangerine", 0.80m, 0.00m },
                    { 172, 29.00m, 9.00m, 10, null, 0.30m, 2.80m, 60.00m, null, "Lemon", 1.10m, 0.00m },
                    { 173, 30.00m, 11.00m, 10, null, 0.20m, 2.80m, 50.00m, null, "Lime", 0.70m, 0.00m },
                    { 174, 30.00m, 8.00m, 1, null, 0.20m, 0.40m, null, null, "Watermelon", 0.60m, 0.00m },
                    { 175, 34.00m, 8.00m, 1, null, 0.20m, 0.90m, null, null, "Cantaloupe", 0.80m, 0.02m },
                    { 176, 68.00m, 14.00m, 10, null, 1.00m, 5.40m, 100.00m, null, "Guava", 2.60m, 0.00m },
                    { 177, 43.00m, 11.00m, 1, null, 0.30m, 1.70m, null, null, "Pawpaw", 0.50m, 0.01m },
                    { 178, 354.00m, 15.00m, 1, null, 33.00m, 9.00m, null, null, "Coconut (fresh meat)", 3.30m, 0.02m },
                    { 179, 230.00m, 5.50m, 4, 1.03m, 24.00m, 0.00m, null, null, "Coconut milk", 2.30m, 0.02m },
                    { 180, 260.00m, 14.00m, 1, null, 22.00m, 8.00m, null, null, "Palm fruit", 2.00m, 0.00m },
                    { 181, 130.00m, 28.00m, 1, null, 0.30m, 0.40m, null, null, "Rice (white)", 2.70m, 0.00m },
                    { 182, 123.00m, 25.00m, 1, null, 0.90m, 1.80m, null, null, "Rice (brown)", 2.70m, 0.00m },
                    { 183, 132.00m, 23.00m, 1, null, 0.50m, 7.00m, null, null, "Beans (black)", 8.00m, 0.01m },
                    { 184, 139.00m, 25.00m, 1, null, 0.50m, 6.00m, null, null, "Beans (red)", 8.00m, 0.01m },
                    { 185, 116.00m, 20.00m, 1, null, 0.50m, 6.00m, null, null, "Cowpeas (niébé)", 8.00m, 0.01m },
                    { 186, 567.00m, 16.10m, 1, null, 49.20m, 8.50m, null, null, "Groundnuts (peanuts) - raw", 25.80m, 0.02m },
                    { 187, 587.00m, 21.00m, 1, null, 50.00m, 9.00m, null, null, "Ground peanuts (roasted)", 24.00m, 0.02m },
                    { 188, 598.00m, 22.00m, 6, 1.10m, 51.00m, 5.00m, null, null, "Peanut butter", 22.00m, 0.50m },
                    { 189, 361.00m, 76.00m, 1, null, 1.50m, 7.00m, null, null, "Cornmeal", 7.00m, 0.01m },
                    { 190, 378.00m, 73.00m, 1, null, 4.20m, 8.50m, null, null, "Millet", 11.00m, 0.01m },
                    { 191, 339.00m, 74.00m, 1, null, 3.30m, 6.70m, null, null, "Sorghum", 11.00m, 0.01m },
                    { 192, 364.00m, 76.00m, 1, null, 1.00m, 2.70m, null, null, "Wheat flour", 10.00m, 0.00m },
                    { 193, 265.00m, 49.00m, 1, null, 3.20m, 2.50m, null, null, "Bread", 9.00m, 0.50m },
                    { 194, 131.00m, 25.00m, 1, null, 1.10m, 1.80m, null, null, "Pasta", 5.00m, 0.01m },
                    { 195, 103.00m, 27.00m, 1, null, 0.20m, 4.90m, null, null, "Breadfruit", 1.10m, 0.00m },
                    { 196, 884.00m, 0.00m, 6, 0.92m, 100.00m, 0.00m, null, null, "Palm oil (red)", 0.00m, 0.00m },
                    { 197, 884.00m, 0.00m, 6, 0.92m, 100.00m, 0.00m, null, null, "Vegetable oil", 0.00m, 0.00m },
                    { 198, 884.00m, 0.00m, 6, 0.92m, 100.00m, 0.00m, null, null, "Olive oil", 0.00m, 0.00m },
                    { 199, 884.00m, 0.00m, 6, 0.92m, 100.00m, 0.00m, null, null, "Groundnut oil", 0.00m, 0.00m },
                    { 200, 717.00m, 0.10m, 6, 0.91m, 81.00m, 0.00m, null, null, "Butter", 0.90m, 0.50m },
                    { 201, 717.00m, 0.20m, 6, 0.91m, 81.00m, 0.00m, null, null, "Margarine", 0.20m, 0.80m },
                    { 202, 724.00m, 0.60m, 6, 0.92m, 79.00m, 0.00m, null, null, "Mayonnaise", 1.00m, 0.50m },
                    { 203, 884.00m, 0.00m, 6, 0.92m, 100.00m, 0.00m, null, null, "Palm kernel oil", 0.00m, 0.00m },
                    { 204, 0.00m, 0.00m, 7, 1.20m, 0.00m, 0.00m, null, null, "Salt", 0.00m, 38.76m },
                    { 205, 255.00m, 50.00m, 7, 0.50m, 3.00m, 25.00m, 2.00m, null, "Black pepper", 10.00m, 0.02m },
                    { 206, 255.00m, 50.00m, 7, 0.50m, 2.00m, 25.00m, 2.00m, null, "White pepper", 10.00m, 0.02m },
                    { 207, 200.00m, 15.00m, 9, null, 12.00m, 0.00m, 10.00m, null, "Maggi cube", 8.00m, 15.00m },
                    { 208, 341.00m, 79.00m, 7, 0.40m, 1.00m, 9.00m, 2.00m, null, "Onion powder", 10.00m, 0.05m },
                    { 209, 331.00m, 72.00m, 7, 0.40m, 0.70m, 9.00m, 2.00m, null, "Garlic powder", 16.00m, 0.05m },
                    { 210, 80.00m, 18.00m, 1, null, 0.80m, 2.00m, 10.00m, null, "Ginger (fresh)", 1.80m, 0.01m },
                    { 211, 335.00m, 71.00m, 7, 0.40m, 4.00m, 14.00m, 2.00m, null, "Ginger powder", 9.00m, 0.03m },
                    { 212, 276.00m, 63.00m, 7, 0.30m, 4.00m, 37.00m, 1.00m, null, "Thyme (dried)", 9.00m, 0.02m },
                    { 213, 313.00m, 75.00m, 7, 0.30m, 8.00m, 26.00m, 1.00m, null, "Bay leaves", 7.60m, 0.02m },
                    { 214, 557.00m, 15.00m, 1, null, 47.00m, 3.00m, null, null, "Egusi seeds (melon seeds)", 28.00m, 0.03m },
                    { 215, 553.00m, 30.00m, 1, null, 44.00m, 3.00m, null, null, "Cashew nuts", 18.00m, 0.01m },
                    { 216, 650.00m, 15.00m, 1, null, 62.00m, 8.00m, 5.00m, null, "African walnut (Coula edulis)", 12.00m, 0.01m },
                    { 217, 620.00m, 10.00m, 1, null, 55.00m, 6.00m, 3.00m, null, "Conophor nuts (Tetracarpidium)", 18.00m, 0.01m },
                    { 218, 573.00m, 23.00m, 1, null, 50.00m, 12.00m, null, null, "Sesame seeds", 18.00m, 0.01m },
                    { 219, 559.00m, 10.00m, 1, null, 49.00m, 6.00m, null, null, "Pumpkin seeds", 30.00m, 0.01m },
                    { 220, 584.00m, 20.00m, 1, null, 51.00m, 9.00m, null, null, "Sunflower seeds", 20.00m, 0.01m },
                    { 221, 155.00m, 1.10m, 9, null, 11.00m, 0.00m, 50.00m, null, "Egg (chicken)", 13.00m, 0.12m },
                    { 222, 42.00m, 5.00m, 4, 1.03m, 1.00m, 0.00m, null, null, "Milk (fresh)", 3.40m, 0.04m },
                    { 223, 496.00m, 38.00m, 1, null, 27.00m, 0.00m, null, null, "Milk (powdered)", 26.00m, 0.40m },
                    { 224, 61.00m, 4.70m, 1, null, 3.30m, 0.00m, null, null, "Yogurt (plain)", 3.50m, 0.05m },
                    { 225, 300.00m, 2.00m, 1, null, 24.00m, 0.00m, null, null, "Cheese (processed)", 18.00m, 1.50m }
                });

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

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "100" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_IngredientUnitId",
                table: "Ingredients",
                column: "IngredientUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_UnitId",
                table: "RecipeIngredients",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeInstructions_RecipeId",
                table: "RecipeInstructions",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CategoryId",
                table: "Recipes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UserId",
                table: "Recipes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "RecipeInstructions");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "IngredientUnits");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RecipeCategories");
        }
    }
}
