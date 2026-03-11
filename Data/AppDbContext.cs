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
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeInstruction> RecipeInstructions { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeLike> RecipeLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRole = new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            //configuration of recipelikes
            builder.Entity<RecipeLike>()
                .HasIndex(rl => new { rl.RecipeId, rl.UserId })
                .IsUnique();

            builder.Entity<RecipeLike>()
                .HasOne(rl => rl.Recipe)
                .WithMany(r => r.RecipeLikes)
                .HasForeignKey(r => r.RecipeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RecipeLike>()
                .HasOne(rl => rl.User)
                .WithMany()
                .HasForeignKey(rl => rl.UserId)
                .OnDelete(DeleteBehavior.NoAction);

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

            // Seed Ingredients
            builder.Entity<Ingredient>().HasData(
                // Category 1: Meats & Poultry (15 items)
                new Ingredient { Id = 101, Name = "Beef", DefaultUnitId = 1, CaloriesPer100g = 250.00m, Proteins = 26.00m, Carbohydrates = 0.00m, Fats = 20.00m, Fibers = 0.00m, Sodium = 0.05m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 102, Name = "Chicken breast", DefaultUnitId = 1, CaloriesPer100g = 165.00m, Proteins = 31.00m, Carbohydrates = 0.00m, Fats = 3.60m, Fibers = 0.00m, Sodium = 0.07m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 103, Name = "Chicken thighs", DefaultUnitId = 1, CaloriesPer100g = 209.00m, Proteins = 26.00m, Carbohydrates = 0.00m, Fats = 11.00m, Fibers = 0.00m, Sodium = 0.08m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 104, Name = "Chicken drumsticks", DefaultUnitId = 1, CaloriesPer100g = 172.00m, Proteins = 28.00m, Carbohydrates = 0.00m, Fats = 6.00m, Fibers = 0.00m, Sodium = 0.09m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 105, Name = "Goat meat", DefaultUnitId = 1, CaloriesPer100g = 143.00m, Proteins = 27.00m, Carbohydrates = 0.00m, Fats = 3.00m, Fibers = 0.00m, Sodium = 0.08m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 106, Name = "Pork", DefaultUnitId = 1, CaloriesPer100g = 242.00m, Proteins = 27.00m, Carbohydrates = 0.00m, Fats = 14.00m, Fibers = 0.00m, Sodium = 0.06m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 107, Name = "Pork ribs", DefaultUnitId = 1, CaloriesPer100g = 321.00m, Proteins = 16.00m, Carbohydrates = 0.00m, Fats = 28.00m, Fibers = 0.00m, Sodium = 0.07m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 108, Name = "Turkey", DefaultUnitId = 1, CaloriesPer100g = 135.00m, Proteins = 29.00m, Carbohydrates = 0.00m, Fats = 1.00m, Fibers = 0.00m, Sodium = 0.07m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 109, Name = "Duck", DefaultUnitId = 1, CaloriesPer100g = 337.00m, Proteins = 19.00m, Carbohydrates = 0.00m, Fats = 28.00m, Fibers = 0.00m, Sodium = 0.07m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 110, Name = "Rabbit", DefaultUnitId = 1, CaloriesPer100g = 136.00m, Proteins = 25.00m, Carbohydrates = 0.00m, Fats = 3.50m, Fibers = 0.00m, Sodium = 0.05m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 111, Name = "Lamb", DefaultUnitId = 1, CaloriesPer100g = 258.00m, Proteins = 25.00m, Carbohydrates = 0.00m, Fats = 17.00m, Fibers = 0.00m, Sodium = 0.07m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 112, Name = "Beef liver", DefaultUnitId = 1, CaloriesPer100g = 135.00m, Proteins = 20.00m, Carbohydrates = 4.00m, Fats = 3.60m, Fibers = 0.00m, Sodium = 0.07m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 113, Name = "Chicken liver", DefaultUnitId = 1, CaloriesPer100g = 119.00m, Proteins = 17.00m, Carbohydrates = 1.00m, Fats = 5.00m, Fibers = 0.00m, Sodium = 0.07m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 114, Name = "Beef tripe", DefaultUnitId = 1, CaloriesPer100g = 85.00m, Proteins = 12.00m, Carbohydrates = 0.00m, Fats = 3.70m, Fibers = 0.00m, Sodium = 0.06m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 115, Name = "Smoked beef", DefaultUnitId = 1, CaloriesPer100g = 290.00m, Proteins = 28.00m, Carbohydrates = 0.00m, Fats = 19.00m, Fibers = 0.00m, Sodium = 1.20m, GramsPerUnit = null, Density = null },

                // Category 2: Fish & Seafood (15 items)
                new Ingredient { Id = 116, Name = "Fresh fish (tilapia)", DefaultUnitId = 1, CaloriesPer100g = 128.00m, Proteins = 26.00m, Carbohydrates = 0.00m, Fats = 2.70m, Fibers = 0.00m, Sodium = 0.06m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 117, Name = "Mackerel (maquereau)", DefaultUnitId = 1, CaloriesPer100g = 205.00m, Proteins = 19.00m, Carbohydrates = 0.00m, Fats = 14.00m, Fibers = 0.00m, Sodium = 0.08m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 118, Name = "Catfish (poisson-chat)", DefaultUnitId = 1, CaloriesPer100g = 162.00m, Proteins = 18.00m, Carbohydrates = 0.00m, Fats = 9.00m, Fibers = 0.00m, Sodium = 0.06m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 119, Name = "Croaker (corb)", DefaultUnitId = 1, CaloriesPer100g = 150.00m, Proteins = 22.00m, Carbohydrates = 0.00m, Fats = 6.00m, Fibers = 0.00m, Sodium = 0.07m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 120, Name = "Barracuda", DefaultUnitId = 1, CaloriesPer100g = 175.00m, Proteins = 24.00m, Carbohydrates = 0.00m, Fats = 8.00m, Fibers = 0.00m, Sodium = 0.08m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 121, Name = "Bonga fish (poisson bonga)", DefaultUnitId = 1, CaloriesPer100g = 158.00m, Proteins = 21.00m, Carbohydrates = 0.00m, Fats = 8.00m, Fibers = 0.00m, Sodium = 0.09m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 122, Name = "Sardines", DefaultUnitId = 1, CaloriesPer100g = 208.00m, Proteins = 25.00m, Carbohydrates = 0.00m, Fats = 11.00m, Fibers = 0.00m, Sodium = 0.05m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 123, Name = "Tuna", DefaultUnitId = 1, CaloriesPer100g = 184.00m, Proteins = 30.00m, Carbohydrates = 0.00m, Fats = 6.00m, Fibers = 0.00m, Sodium = 0.05m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 124, Name = "Snapper (vivaneau)", DefaultUnitId = 1, CaloriesPer100g = 128.00m, Proteins = 26.00m, Carbohydrates = 0.00m, Fats = 2.00m, Fibers = 0.00m, Sodium = 0.06m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 125, Name = "Smoked fish", DefaultUnitId = 1, CaloriesPer100g = 190.00m, Proteins = 28.00m, Carbohydrates = 0.00m, Fats = 8.00m, Fibers = 0.00m, Sodium = 1.50m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 126, Name = "Dried fish", DefaultUnitId = 1, CaloriesPer100g = 280.00m, Proteins = 50.00m, Carbohydrates = 0.00m, Fats = 8.00m, Fibers = 0.00m, Sodium = 2.00m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 127, Name = "Shrimp (fresh)", DefaultUnitId = 1, CaloriesPer100g = 85.00m, Proteins = 20.00m, Carbohydrates = 0.00m, Fats = 0.50m, Fibers = 0.00m, Sodium = 0.30m, GramsPerUnit = 10.00m, Density = null },
                new Ingredient { Id = 128, Name = "Dried shrimp", DefaultUnitId = 1, CaloriesPer100g = 250.00m, Proteins = 50.00m, Carbohydrates = 0.00m, Fats = 5.00m, Fibers = 0.00m, Sodium = 2.50m, GramsPerUnit = 2.00m, Density = null },
                new Ingredient { Id = 129, Name = "Crab", DefaultUnitId = 1, CaloriesPer100g = 87.00m, Proteins = 18.00m, Carbohydrates = 0.00m, Fats = 1.50m, Fibers = 0.00m, Sodium = 0.30m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 130, Name = "Crayfish", DefaultUnitId = 1, CaloriesPer100g = 110.00m, Proteins = 22.00m, Carbohydrates = 0.00m, Fats = 2.00m, Fibers = 0.00m, Sodium = 0.20m, GramsPerUnit = 5.00m, Density = null },

                // Category 3: Vegetables & Leaves (20 items)
                new Ingredient { Id = 131, Name = "Ndolé leaves (bitter leaf)", DefaultUnitId = 1, CaloriesPer100g = 45.00m, Proteins = 4.50m, Carbohydrates = 5.00m, Fats = 1.00m, Fibers = 4.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 132, Name = "Eru leaves", DefaultUnitId = 1, CaloriesPer100g = 50.00m, Proteins = 4.00m, Carbohydrates = 6.00m, Fats = 1.50m, Fibers = 3.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 133, Name = "Okra (gombo)", DefaultUnitId = 1, CaloriesPer100g = 33.00m, Proteins = 2.00m, Carbohydrates = 7.00m, Fats = 0.20m, Fibers = 3.20m, Sodium = 0.01m, GramsPerUnit = 15.00m, Density = null },
                new Ingredient { Id = 134, Name = "Waterleaf", DefaultUnitId = 1, CaloriesPer100g = 23.00m, Proteins = 2.50m, Carbohydrates = 3.00m, Fats = 0.40m, Fibers = 1.50m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 135, Name = "Huckleberry leaves (folong)", DefaultUnitId = 1, CaloriesPer100g = 42.00m, Proteins = 4.00m, Carbohydrates = 5.00m, Fats = 0.80m, Fibers = 3.50m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 136, Name = "Cassava leaves", DefaultUnitId = 1, CaloriesPer100g = 91.00m, Proteins = 7.00m, Carbohydrates = 13.00m, Fats = 2.00m, Fibers = 5.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 137, Name = "Pumpkin leaves", DefaultUnitId = 1, CaloriesPer100g = 32.00m, Proteins = 3.00m, Carbohydrates = 5.00m, Fats = 0.40m, Fibers = 2.50m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 138, Name = "Spinach", DefaultUnitId = 1, CaloriesPer100g = 23.00m, Proteins = 2.90m, Carbohydrates = 3.60m, Fats = 0.40m, Fibers = 2.20m, Sodium = 0.08m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 139, Name = "Cabbage", DefaultUnitId = 1, CaloriesPer100g = 25.00m, Proteins = 1.30m, Carbohydrates = 5.80m, Fats = 0.10m, Fibers = 2.50m, Sodium = 0.02m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 140, Name = "Lettuce", DefaultUnitId = 1, CaloriesPer100g = 15.00m, Proteins = 1.40m, Carbohydrates = 2.90m, Fats = 0.20m, Fibers = 1.30m, Sodium = 0.03m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 141, Name = "Carrot", DefaultUnitId = 10, CaloriesPer100g = 41.00m, Proteins = 0.90m, Carbohydrates = 9.60m, Fats = 0.20m, Fibers = 2.80m, Sodium = 0.07m, GramsPerUnit = 60.00m, Density = null },
                new Ingredient { Id = 142, Name = "Green bell pepper", DefaultUnitId = 10, CaloriesPer100g = 20.00m, Proteins = 0.90m, Carbohydrates = 4.60m, Fats = 0.20m, Fibers = 1.70m, Sodium = 0.00m, GramsPerUnit = 120.00m, Density = null },
                new Ingredient { Id = 143, Name = "Red bell pepper", DefaultUnitId = 10, CaloriesPer100g = 31.00m, Proteins = 1.00m, Carbohydrates = 6.00m, Fats = 0.30m, Fibers = 2.10m, Sodium = 0.00m, GramsPerUnit = 120.00m, Density = null },
                new Ingredient { Id = 144, Name = "Hot pepper (piment)", DefaultUnitId = 13, CaloriesPer100g = 40.00m, Proteins = 1.90m, Carbohydrates = 8.80m, Fats = 0.40m, Fibers = 1.50m, Sodium = 0.01m, GramsPerUnit = 10.00m, Density = null },
                new Ingredient { Id = 145, Name = "African eggplant (aubergine)", DefaultUnitId = 10, CaloriesPer100g = 25.00m, Proteins = 1.00m, Carbohydrates = 5.70m, Fats = 0.20m, Fibers = 3.00m, Sodium = 0.00m, GramsPerUnit = 80.00m, Density = null },
                new Ingredient { Id = 146, Name = "Zucchini", DefaultUnitId = 10, CaloriesPer100g = 17.00m, Proteins = 1.20m, Carbohydrates = 3.10m, Fats = 0.30m, Fibers = 1.00m, Sodium = 0.01m, GramsPerUnit = 200.00m, Density = null },
                new Ingredient { Id = 147, Name = "Green beans", DefaultUnitId = 1, CaloriesPer100g = 31.00m, Proteins = 1.80m, Carbohydrates = 7.00m, Fats = 0.20m, Fibers = 3.40m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 148, Name = "Peas", DefaultUnitId = 1, CaloriesPer100g = 81.00m, Proteins = 5.40m, Carbohydrates = 14.40m, Fats = 0.40m, Fibers = 5.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 149, Name = "Corn (maize)", DefaultUnitId = 10, CaloriesPer100g = 86.00m, Proteins = 3.20m, Carbohydrates = 19.00m, Fats = 1.20m, Fibers = 2.70m, Sodium = 0.02m, GramsPerUnit = 150.00m, Density = null },
                new Ingredient { Id = 150, Name = "Mushrooms", DefaultUnitId = 1, CaloriesPer100g = 22.00m, Proteins = 3.10m, Carbohydrates = 3.30m, Fats = 0.30m, Fibers = 1.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },

                // Category 4: Tubers & Starches (15 items)
                new Ingredient { Id = 151, Name = "Cassava (fresh)", DefaultUnitId = 1, CaloriesPer100g = 160.00m, Proteins = 1.40m, Carbohydrates = 38.00m, Fats = 0.30m, Fibers = 1.80m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 152, Name = "Cassava (frozen)", DefaultUnitId = 1, CaloriesPer100g = 150.00m, Proteins = 1.30m, Carbohydrates = 36.00m, Fats = 0.30m, Fibers = 1.70m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 153, Name = "Yam", DefaultUnitId = 1, CaloriesPer100g = 118.00m, Proteins = 1.50m, Carbohydrates = 28.00m, Fats = 0.20m, Fibers = 4.10m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 154, Name = "Sweet potato (orange)", DefaultUnitId = 10, CaloriesPer100g = 86.00m, Proteins = 1.60m, Carbohydrates = 20.00m, Fats = 0.10m, Fibers = 3.00m, Sodium = 0.06m, GramsPerUnit = 150.00m, Density = null },
                new Ingredient { Id = 155, Name = "Sweet potato (white)", DefaultUnitId = 10, CaloriesPer100g = 90.00m, Proteins = 1.60m, Carbohydrates = 21.00m, Fats = 0.10m, Fibers = 2.50m, Sodium = 0.06m, GramsPerUnit = 150.00m, Density = null },
                new Ingredient { Id = 156, Name = "Cocoyam (macabo)", DefaultUnitId = 1, CaloriesPer100g = 112.00m, Proteins = 1.50m, Carbohydrates = 26.00m, Fats = 0.20m, Fibers = 2.50m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 157, Name = "Taro (pomme de terre)", DefaultUnitId = 1, CaloriesPer100g = 112.00m, Proteins = 1.50m, Carbohydrates = 26.00m, Fats = 0.20m, Fibers = 4.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 158, Name = "Potato (Irish potato)", DefaultUnitId = 10, CaloriesPer100g = 77.00m, Proteins = 2.00m, Carbohydrates = 17.00m, Fats = 0.10m, Fibers = 2.20m, Sodium = 0.01m, GramsPerUnit = 150.00m, Density = null },
                new Ingredient { Id = 159, Name = "Plantain (green/unripe)", DefaultUnitId = 10, CaloriesPer100g = 116.00m, Proteins = 1.20m, Carbohydrates = 31.00m, Fats = 0.40m, Fibers = 2.30m, Sodium = 0.00m, GramsPerUnit = 200.00m, Density = null },
                new Ingredient { Id = 160, Name = "Plantain (ripe/yellow)", DefaultUnitId = 10, CaloriesPer100g = 122.00m, Proteins = 1.30m, Carbohydrates = 32.00m, Fats = 0.40m, Fibers = 2.30m, Sodium = 0.00m, GramsPerUnit = 200.00m, Density = null },
                new Ingredient { Id = 161, Name = "Plantain (very ripe/black)", DefaultUnitId = 10, CaloriesPer100g = 130.00m, Proteins = 1.30m, Carbohydrates = 34.00m, Fats = 0.40m, Fibers = 2.00m, Sodium = 0.00m, GramsPerUnit = 200.00m, Density = null },
                new Ingredient { Id = 162, Name = "Banana (dessert)", DefaultUnitId = 10, CaloriesPer100g = 89.00m, Proteins = 1.10m, Carbohydrates = 22.80m, Fats = 0.30m, Fibers = 2.60m, Sodium = 0.00m, GramsPerUnit = 120.00m, Density = null },
                new Ingredient { Id = 163, Name = "Fufu flour (corn)", DefaultUnitId = 1, CaloriesPer100g = 350.00m, Proteins = 3.00m, Carbohydrates = 80.00m, Fats = 1.00m, Fibers = 2.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 164, Name = "Fufu flour (cassava)", DefaultUnitId = 1, CaloriesPer100g = 360.00m, Proteins = 1.00m, Carbohydrates = 88.00m, Fats = 0.50m, Fibers = 2.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 165, Name = "Garri (cassava granules)", DefaultUnitId = 1, CaloriesPer100g = 350.00m, Proteins = 1.50m, Carbohydrates = 85.00m, Fats = 0.50m, Fibers = 3.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },

                // Category 5: Fruits (15 items)
                new Ingredient { Id = 166, Name = "Mango", DefaultUnitId = 10, CaloriesPer100g = 60.00m, Proteins = 0.80m, Carbohydrates = 15.00m, Fats = 0.40m, Fibers = 1.60m, Sodium = 0.00m, GramsPerUnit = 200.00m, Density = null },
                new Ingredient { Id = 167, Name = "Pineapple", DefaultUnitId = 1, CaloriesPer100g = 50.00m, Proteins = 0.50m, Carbohydrates = 13.00m, Fats = 0.10m, Fibers = 1.40m, Sodium = 0.00m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 168, Name = "Papaya", DefaultUnitId = 1, CaloriesPer100g = 43.00m, Proteins = 0.50m, Carbohydrates = 11.00m, Fats = 0.30m, Fibers = 1.70m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 169, Name = "Avocado", DefaultUnitId = 10, CaloriesPer100g = 160.00m, Proteins = 2.00m, Carbohydrates = 8.50m, Fats = 15.00m, Fibers = 6.70m, Sodium = 0.01m, GramsPerUnit = 150.00m, Density = null },
                new Ingredient { Id = 170, Name = "Orange", DefaultUnitId = 10, CaloriesPer100g = 47.00m, Proteins = 0.90m, Carbohydrates = 12.00m, Fats = 0.10m, Fibers = 2.40m, Sodium = 0.00m, GramsPerUnit = 130.00m, Density = null },
                new Ingredient { Id = 171, Name = "Tangerine", DefaultUnitId = 10, CaloriesPer100g = 53.00m, Proteins = 0.80m, Carbohydrates = 13.00m, Fats = 0.30m, Fibers = 1.80m, Sodium = 0.00m, GramsPerUnit = 80.00m, Density = null },
                new Ingredient { Id = 172, Name = "Lemon", DefaultUnitId = 10, CaloriesPer100g = 29.00m, Proteins = 1.10m, Carbohydrates = 9.00m, Fats = 0.30m, Fibers = 2.80m, Sodium = 0.00m, GramsPerUnit = 60.00m, Density = null },
                new Ingredient { Id = 173, Name = "Lime", DefaultUnitId = 10, CaloriesPer100g = 30.00m, Proteins = 0.70m, Carbohydrates = 11.00m, Fats = 0.20m, Fibers = 2.80m, Sodium = 0.00m, GramsPerUnit = 50.00m, Density = null },
                new Ingredient { Id = 174, Name = "Watermelon", DefaultUnitId = 1, CaloriesPer100g = 30.00m, Proteins = 0.60m, Carbohydrates = 8.00m, Fats = 0.20m, Fibers = 0.40m, Sodium = 0.00m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 175, Name = "Cantaloupe", DefaultUnitId = 1, CaloriesPer100g = 34.00m, Proteins = 0.80m, Carbohydrates = 8.00m, Fats = 0.20m, Fibers = 0.90m, Sodium = 0.02m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 176, Name = "Guava", DefaultUnitId = 10, CaloriesPer100g = 68.00m, Proteins = 2.60m, Carbohydrates = 14.00m, Fats = 1.00m, Fibers = 5.40m, Sodium = 0.00m, GramsPerUnit = 100.00m, Density = null },
                new Ingredient { Id = 177, Name = "Pawpaw", DefaultUnitId = 1, CaloriesPer100g = 43.00m, Proteins = 0.50m, Carbohydrates = 11.00m, Fats = 0.30m, Fibers = 1.70m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 178, Name = "Coconut (fresh meat)", DefaultUnitId = 1, CaloriesPer100g = 354.00m, Proteins = 3.30m, Carbohydrates = 15.00m, Fats = 33.00m, Fibers = 9.00m, Sodium = 0.02m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 179, Name = "Coconut milk", DefaultUnitId = 4, CaloriesPer100g = 230.00m, Proteins = 2.30m, Carbohydrates = 5.50m, Fats = 24.00m, Fibers = 0.00m, Sodium = 0.02m, GramsPerUnit = null, Density = 1.03m },
                new Ingredient { Id = 180, Name = "Palm fruit", DefaultUnitId = 1, CaloriesPer100g = 260.00m, Proteins = 2.00m, Carbohydrates = 14.00m, Fats = 22.00m, Fibers = 8.00m, Sodium = 0.00m, GramsPerUnit = null, Density = null },

                // Category 6: Grains & Legumes (15 items)
                new Ingredient { Id = 181, Name = "Rice (white)", DefaultUnitId = 1, CaloriesPer100g = 130.00m, Proteins = 2.70m, Carbohydrates = 28.00m, Fats = 0.30m, Fibers = 0.40m, Sodium = 0.00m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 182, Name = "Rice (brown)", DefaultUnitId = 1, CaloriesPer100g = 123.00m, Proteins = 2.70m, Carbohydrates = 25.00m, Fats = 0.90m, Fibers = 1.80m, Sodium = 0.00m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 183, Name = "Beans (black)", DefaultUnitId = 1, CaloriesPer100g = 132.00m, Proteins = 8.00m, Carbohydrates = 23.00m, Fats = 0.50m, Fibers = 7.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 184, Name = "Beans (red)", DefaultUnitId = 1, CaloriesPer100g = 139.00m, Proteins = 8.00m, Carbohydrates = 25.00m, Fats = 0.50m, Fibers = 6.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 185, Name = "Cowpeas (niébé)", DefaultUnitId = 1, CaloriesPer100g = 116.00m, Proteins = 8.00m, Carbohydrates = 20.00m, Fats = 0.50m, Fibers = 6.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 186, Name = "Groundnuts (peanuts) - raw", DefaultUnitId = 1, CaloriesPer100g = 567.00m, Proteins = 25.80m, Carbohydrates = 16.10m, Fats = 49.20m, Fibers = 8.50m, Sodium = 0.02m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 187, Name = "Ground peanuts (roasted)", DefaultUnitId = 1, CaloriesPer100g = 587.00m, Proteins = 24.00m, Carbohydrates = 21.00m, Fats = 50.00m, Fibers = 9.00m, Sodium = 0.02m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 188, Name = "Peanut butter", DefaultUnitId = 6, CaloriesPer100g = 598.00m, Proteins = 22.00m, Carbohydrates = 22.00m, Fats = 51.00m, Fibers = 5.00m, Sodium = 0.50m, GramsPerUnit = null, Density = 1.10m },
                new Ingredient { Id = 189, Name = "Cornmeal", DefaultUnitId = 1, CaloriesPer100g = 361.00m, Proteins = 7.00m, Carbohydrates = 76.00m, Fats = 1.50m, Fibers = 7.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 190, Name = "Millet", DefaultUnitId = 1, CaloriesPer100g = 378.00m, Proteins = 11.00m, Carbohydrates = 73.00m, Fats = 4.20m, Fibers = 8.50m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 191, Name = "Sorghum", DefaultUnitId = 1, CaloriesPer100g = 339.00m, Proteins = 11.00m, Carbohydrates = 74.00m, Fats = 3.30m, Fibers = 6.70m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 192, Name = "Wheat flour", DefaultUnitId = 1, CaloriesPer100g = 364.00m, Proteins = 10.00m, Carbohydrates = 76.00m, Fats = 1.00m, Fibers = 2.70m, Sodium = 0.00m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 193, Name = "Bread", DefaultUnitId = 1, CaloriesPer100g = 265.00m, Proteins = 9.00m, Carbohydrates = 49.00m, Fats = 3.20m, Fibers = 2.50m, Sodium = 0.50m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 194, Name = "Pasta", DefaultUnitId = 1, CaloriesPer100g = 131.00m, Proteins = 5.00m, Carbohydrates = 25.00m, Fats = 1.10m, Fibers = 1.80m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 195, Name = "Breadfruit", DefaultUnitId = 1, CaloriesPer100g = 103.00m, Proteins = 1.10m, Carbohydrates = 27.00m, Fats = 0.20m, Fibers = 4.90m, Sodium = 0.00m, GramsPerUnit = null, Density = null },

                // Category 7: Oils & Fats (8 items)
                new Ingredient { Id = 196, Name = "Palm oil (red)", DefaultUnitId = 6, CaloriesPer100g = 884.00m, Proteins = 0.00m, Carbohydrates = 0.00m, Fats = 100.00m, Fibers = 0.00m, Sodium = 0.00m, GramsPerUnit = null, Density = 0.92m },
                new Ingredient { Id = 197, Name = "Vegetable oil", DefaultUnitId = 6, CaloriesPer100g = 884.00m, Proteins = 0.00m, Carbohydrates = 0.00m, Fats = 100.00m, Fibers = 0.00m, Sodium = 0.00m, GramsPerUnit = null, Density = 0.92m },
                new Ingredient { Id = 198, Name = "Olive oil", DefaultUnitId = 6, CaloriesPer100g = 884.00m, Proteins = 0.00m, Carbohydrates = 0.00m, Fats = 100.00m, Fibers = 0.00m, Sodium = 0.00m, GramsPerUnit = null, Density = 0.92m },
                new Ingredient { Id = 199, Name = "Groundnut oil", DefaultUnitId = 6, CaloriesPer100g = 884.00m, Proteins = 0.00m, Carbohydrates = 0.00m, Fats = 100.00m, Fibers = 0.00m, Sodium = 0.00m, GramsPerUnit = null, Density = 0.92m },
                new Ingredient { Id = 200, Name = "Butter", DefaultUnitId = 6, CaloriesPer100g = 717.00m, Proteins = 0.90m, Carbohydrates = 0.10m, Fats = 81.00m, Fibers = 0.00m, Sodium = 0.50m, GramsPerUnit = null, Density = 0.91m },
                new Ingredient { Id = 201, Name = "Margarine", DefaultUnitId = 6, CaloriesPer100g = 717.00m, Proteins = 0.20m, Carbohydrates = 0.20m, Fats = 81.00m, Fibers = 0.00m, Sodium = 0.80m, GramsPerUnit = null, Density = 0.91m },
                new Ingredient { Id = 202, Name = "Mayonnaise", DefaultUnitId = 6, CaloriesPer100g = 724.00m, Proteins = 1.00m, Carbohydrates = 0.60m, Fats = 79.00m, Fibers = 0.00m, Sodium = 0.50m, GramsPerUnit = null, Density = 0.92m },
                new Ingredient { Id = 203, Name = "Palm kernel oil", DefaultUnitId = 6, CaloriesPer100g = 884.00m, Proteins = 0.00m, Carbohydrates = 0.00m, Fats = 100.00m, Fibers = 0.00m, Sodium = 0.00m, GramsPerUnit = null, Density = 0.92m },

                // Category 8: Spices & Seasonings (10 items)
                new Ingredient { Id = 204, Name = "Salt", DefaultUnitId = 7, CaloriesPer100g = 0.00m, Proteins = 0.00m, Carbohydrates = 0.00m, Fats = 0.00m, Fibers = 0.00m, Sodium = 38.76m, GramsPerUnit = null, Density = 1.20m },
                new Ingredient { Id = 205, Name = "Black pepper", DefaultUnitId = 7, CaloriesPer100g = 255.00m, Proteins = 10.00m, Carbohydrates = 50.00m, Fats = 3.00m, Fibers = 25.00m, Sodium = 0.02m, GramsPerUnit = 2.00m, Density = 0.50m },
                new Ingredient { Id = 206, Name = "White pepper", DefaultUnitId = 7, CaloriesPer100g = 255.00m, Proteins = 10.00m, Carbohydrates = 50.00m, Fats = 2.00m, Fibers = 25.00m, Sodium = 0.02m, GramsPerUnit = 2.00m, Density = 0.50m },
                new Ingredient { Id = 207, Name = "Maggi cube", DefaultUnitId = 9, CaloriesPer100g = 200.00m, Proteins = 8.00m, Carbohydrates = 15.00m, Fats = 12.00m, Fibers = 0.00m, Sodium = 15.00m, GramsPerUnit = 10.00m, Density = null },
                new Ingredient { Id = 208, Name = "Onion powder", DefaultUnitId = 7, CaloriesPer100g = 341.00m, Proteins = 10.00m, Carbohydrates = 79.00m, Fats = 1.00m, Fibers = 9.00m, Sodium = 0.05m, GramsPerUnit = 2.00m, Density = 0.40m },
                new Ingredient { Id = 209, Name = "Garlic powder", DefaultUnitId = 7, CaloriesPer100g = 331.00m, Proteins = 16.00m, Carbohydrates = 72.00m, Fats = 0.70m, Fibers = 9.00m, Sodium = 0.05m, GramsPerUnit = 2.00m, Density = 0.40m },
                new Ingredient { Id = 210, Name = "Ginger (fresh)", DefaultUnitId = 1, CaloriesPer100g = 80.00m, Proteins = 1.80m, Carbohydrates = 18.00m, Fats = 0.80m, Fibers = 2.00m, Sodium = 0.01m, GramsPerUnit = 10.00m, Density = null },
                new Ingredient { Id = 211, Name = "Ginger powder", DefaultUnitId = 7, CaloriesPer100g = 335.00m, Proteins = 9.00m, Carbohydrates = 71.00m, Fats = 4.00m, Fibers = 14.00m, Sodium = 0.03m, GramsPerUnit = 2.00m, Density = 0.40m },
                new Ingredient { Id = 212, Name = "Thyme (dried)", DefaultUnitId = 7, CaloriesPer100g = 276.00m, Proteins = 9.00m, Carbohydrates = 63.00m, Fats = 4.00m, Fibers = 37.00m, Sodium = 0.02m, GramsPerUnit = 1.00m, Density = 0.30m },
                new Ingredient { Id = 213, Name = "Bay leaves", DefaultUnitId = 7, CaloriesPer100g = 313.00m, Proteins = 7.60m, Carbohydrates = 75.00m, Fats = 8.00m, Fibers = 26.00m, Sodium = 0.02m, GramsPerUnit = 1.00m, Density = 0.30m },

                // Category 9: Nuts & Seeds (7 items)
                new Ingredient { Id = 214, Name = "Egusi seeds (melon seeds)", DefaultUnitId = 1, CaloriesPer100g = 557.00m, Proteins = 28.00m, Carbohydrates = 15.00m, Fats = 47.00m, Fibers = 3.00m, Sodium = 0.03m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 215, Name = "Cashew nuts", DefaultUnitId = 1, CaloriesPer100g = 553.00m, Proteins = 18.00m, Carbohydrates = 30.00m, Fats = 44.00m, Fibers = 3.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 216, Name = "African walnut (Coula edulis)", DefaultUnitId = 1, CaloriesPer100g = 650.00m, Proteins = 12.00m, Carbohydrates = 15.00m, Fats = 62.00m, Fibers = 8.00m, Sodium = 0.01m, GramsPerUnit = 5.00m, Density = null },
                new Ingredient { Id = 217, Name = "Conophor nuts (Tetracarpidium)", DefaultUnitId = 1, CaloriesPer100g = 620.00m, Proteins = 18.00m, Carbohydrates = 10.00m, Fats = 55.00m, Fibers = 6.00m, Sodium = 0.01m, GramsPerUnit = 3.00m, Density = null },
                new Ingredient { Id = 218, Name = "Sesame seeds", DefaultUnitId = 1, CaloriesPer100g = 573.00m, Proteins = 18.00m, Carbohydrates = 23.00m, Fats = 50.00m, Fibers = 12.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 219, Name = "Pumpkin seeds", DefaultUnitId = 1, CaloriesPer100g = 559.00m, Proteins = 30.00m, Carbohydrates = 10.00m, Fats = 49.00m, Fibers = 6.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 220, Name = "Sunflower seeds", DefaultUnitId = 1, CaloriesPer100g = 584.00m, Proteins = 20.00m, Carbohydrates = 20.00m, Fats = 51.00m, Fibers = 9.00m, Sodium = 0.01m, GramsPerUnit = null, Density = null },

                // Category 10: Dairy & Eggs (5 items)
                new Ingredient { Id = 221, Name = "Egg (chicken)", DefaultUnitId = 9, CaloriesPer100g = 155.00m, Proteins = 13.00m, Carbohydrates = 1.10m, Fats = 11.00m, Fibers = 0.00m, Sodium = 0.12m, GramsPerUnit = 50.00m, Density = null },
                new Ingredient { Id = 222, Name = "Milk (fresh)", DefaultUnitId = 4, CaloriesPer100g = 42.00m, Proteins = 3.40m, Carbohydrates = 5.00m, Fats = 1.00m, Fibers = 0.00m, Sodium = 0.04m, GramsPerUnit = null, Density = 1.03m },
                new Ingredient { Id = 223, Name = "Milk (powdered)", DefaultUnitId = 1, CaloriesPer100g = 496.00m, Proteins = 26.00m, Carbohydrates = 38.00m, Fats = 27.00m, Fibers = 0.00m, Sodium = 0.40m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 224, Name = "Yogurt (plain)", DefaultUnitId = 1, CaloriesPer100g = 61.00m, Proteins = 3.50m, Carbohydrates = 4.70m, Fats = 3.30m, Fibers = 0.00m, Sodium = 0.05m, GramsPerUnit = null, Density = null },
                new Ingredient { Id = 225, Name = "Cheese (processed)", DefaultUnitId = 1, CaloriesPer100g = 300.00m, Proteins = 18.00m, Carbohydrates = 2.00m, Fats = 24.00m, Fibers = 0.00m, Sodium = 1.50m, GramsPerUnit = null, Density = null }
            );

        }
    }
}
