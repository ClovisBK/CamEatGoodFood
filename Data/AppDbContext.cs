using AuthService.Models;
using AuthService.Models.AppModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class AppDbContext :IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<RecipeCategory> RecipeCategories { get; set; }
        public DbSet<IngredientUnit> IngredientUnits { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRole = new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            builder.Entity<IdentityRole>().HasData(adminRole);

            var adminUser = new ApplicationUser
            {
                Id = "100",
                UserName = "admin@system.com",
                NormalizedUserName = "ADMIN@SYSTEM.COM",
                Email = "wakebeh14@gmail.com",
                NormalizedEmail = "WAKEBEH14@GMAIL.COM",
                EmailConfirmed = true,
                FirstName = "Wa Kebeh",
                LastName = "Mbong",
                Bio = "I am a certified chef that works with tasty foods",
                Location = "Yaounde",
                Gender = "Male",
                ProfilePictureUrl = "myphoto.jpg",
                DateOfBirth = new DateTime(1992, 10, 11),
                Phone = "676455676"
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Password!23");

            builder.Entity<ApplicationUser>().HasData(adminUser);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "100"

                });

            builder.Entity<RecipeCategory>().HasData(
                new RecipeCategory {Id = 1, Name = "Soups", Description = "Traditional Cameroonian soups and delicacies"},
                new RecipeCategory { Id = 2, Name = "Main Dishes", Description = "Complete meals for lunch and dinner"},
                new RecipeCategory { Id = 3, Name = "Breakfast", Description = "Morning meals and small chops"},
                new RecipeCategory { Id = 4, Name = "Snacks", Description = "Small bites and street food"},
                new RecipeCategory { Id = 5, Name = "Beverages", Description = "Drinks and smoothies"}
                );

            builder.Entity<IngredientUnit>().HasData(
                    new IngredientUnit { Id = 1, Name = "gram", Type = "weight", Abbreviation = "g", IsBaseUnit = true },
                    new IngredientUnit { Id = 2, Name = "kilogram", Type = "weight", Abbreviation = "kg", IsBaseUnit = false },
                    new IngredientUnit { Id = 3, Name = "milligram", Type = "weight", Abbreviation = "mg", IsBaseUnit = false },
                    new IngredientUnit { Id = 4, Name = "milliliter", Type = "volume", Abbreviation = "ml", IsBaseUnit = true },
                    new IngredientUnit { Id = 5, Name = "liter", Type = "volume", Abbreviation = "L", IsBaseUnit = false },
                    new IngredientUnit { Id = 6, Name = "tablespoon", Type = "volume", Abbreviation = "tbsp", IsBaseUnit = false },
                    new IngredientUnit { Id = 7, Name = "teaspoon", Type = "volume", Abbreviation = "tsp", IsBaseUnit = false },
                    new IngredientUnit { Id = 8, Name = "cup", Type = "volume", Abbreviation = "cup", IsBaseUnit = false },
                    new IngredientUnit { Id = 9, Name = "piece", Type = "count", Abbreviation = "pc", IsBaseUnit = true },
                    new IngredientUnit { Id = 10, Name = "medium", Type = "count", Abbreviation = "med", IsBaseUnit = false },
                    new IngredientUnit { Id = 11, Name = "large", Type = "count", Abbreviation = "lg", IsBaseUnit = false },
                    new IngredientUnit { Id = 12, Name = "small", Type = "count", Abbreviation = "sm", IsBaseUnit = false },
                    new IngredientUnit { Id = 13, Name = "clove", Type = "count", Abbreviation = "clove", IsBaseUnit = false },
                    new IngredientUnit { Id = 14, Name = "handful", Type = "count", Abbreviation = "hf", IsBaseUnit = false }
             );

            

        }
    }
}
