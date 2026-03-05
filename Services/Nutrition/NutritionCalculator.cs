using AuthService.Models.AppModels;
using AuthService.Services.Nutrition.Models;
using Org.BouncyCastle.Security;

namespace AuthService.Services.Nutrition
{
    public class NutritionCalculator : INutritionCalculator
    {
        private readonly ILogger<NutritionCalculator> _logger;

        public NutritionCalculator(ILogger<NutritionCalculator> logger)
        {
            _logger = logger;
        }
        public NutritionResult CalculateRecipeNutrition(ICollection<RecipeIngredient> recipeIngredients, int servings)
        {
            if(recipeIngredients == null || !recipeIngredients.Any())
            {
                _logger.LogWarning("No ingredients provided for nutrition calculation");
                return new NutritionResult();
            }
            if(servings <= 0)
            {
                _logger.LogWarning("Invalid servings count: {Servings}. Using 1 as default.", servings);
                servings = 1;
            }
            var ingredientConversions = new List<IngredientConversion>();

            foreach(var recipeIngredient in recipeIngredients)
            {
                try
                {
                    var conversion = ConvertIngredientToGrams(recipeIngredient);
                    if(conversion != null)
                    {
                        ingredientConversions.Add(conversion);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to convert ingredient ID {IngredientId} with unit ID {UnitId}", recipeIngredient.IngredientId, recipeIngredient.UnitId);
                }
            }

            var total = SumNutrients(ingredientConversions);

            var perServing = new NutritionValues
            {
                Calories = total.Calories /servings,
                Proteins = total.Proteins / servings,
                Carbohydrates = total.Carbohydrates / servings,
                Fats = total.Fats / servings,
                Fibers = total.Fibers.HasValue ? total.Fibers / servings : null,
                Sodium = total.Sodium.HasValue ? total.Sodium / servings : null
            };

            total.RoundValues();
            perServing.RoundValues();

            return new NutritionResult
            {
                Total = total,
                PerServing = perServing
            };
        }

        private IngredientConversion? ConvertIngredientToGrams(RecipeIngredient recipeIngredient)
        {
            if(recipeIngredient.Ingredient == null)
            {
                _logger.LogWarning("Ingredient data not loaded for RecipeIngredient ID {Id}", recipeIngredient.Id);
                return null;
            }
            if(recipeIngredient.Unit == null)
            {
                _logger.LogWarning("Unit data not loaded for recipeIngredient ID {Id}", recipeIngredient.Id);
                return null;
            }

            var ingredient = recipeIngredient.Ingredient;
            var unit = recipeIngredient.Unit;
            var quantity = recipeIngredient.Quantity;

            decimal grams = ConvertToGrams(quantity, unit, ingredient);

            return new IngredientConversion
            {
                IngredientName = ingredient.Name,
                Grams = grams,
                Calories = CalculateNutrient(grams, ingredient.CaloriesPer100g),
                Proteins = CalculateNutrient(grams, ingredient.Proteins),
                Carbohydrates = CalculateNutrient(grams, ingredient.Carbohydrates),
                Fats = CalculateNutrient(grams, ingredient.Fats),
                Fibers = ingredient.Fibers.HasValue ? CalculateNutrient(grams, ingredient.Fibers.Value) : null,
                Sodium = ingredient.Sodium.HasValue ? CalculateNutrient(grams, ingredient.Sodium.Value) : null
            };
        }
        private decimal ConvertToGrams(decimal quantity, IngredientUnit unit, Ingredient ingredient)
        {
            switch (unit.Type.ToLower())
            {
                case "weight":
                    return ConvertWeightToGrams(quantity, unit);
                case "volume":
                    return ConvertVolumeToGrams(quantity, unit, ingredient.Density);
                case "count":
                    return ConvertCountToGrams(quantity, ingredient.GramsPerUnit);

                default:
                    _logger.LogWarning("Unknown unit type: {UnitType} for unit {UnitName}", unit.Type, unit.Name);
                    return 0;
            }
        }

        private decimal ConvertWeightToGrams(decimal quantity, IngredientUnit unit)
        {
            return unit.Name.ToLower() switch
            {
                "gram" => quantity,
                "kilogram" => quantity * 1000,
                "milligram" => quantity / 1000,
                _ => quantity
            };
        }

        private decimal ConvertVolumeToGrams(decimal quantity, IngredientUnit unit, decimal? density)
        {
            decimal milliliters = unit.Name.ToLower() switch
            {
                "milliliter" => quantity,
                "liter" => quantity * 1000,
                "tablespoon" => quantity * 15,
                "teaspoon" => quantity * 5,
                "cup" => quantity * 240,
                _ => quantity
            };
            if(density.HasValue && density.Value > 0)
            {
                return milliliters * density.Value;
            }
            _logger.LogWarning("No density found for volume-based ingredient. Using 1g/ml as fallback.");
            return milliliters;
        }

        private decimal ConvertCountToGrams(decimal quantity, decimal? gramsPerUnit)
        {
            if(gramsPerUnit.HasValue && gramsPerUnit.Value > 0)
            {
                return quantity * gramsPerUnit.Value;
            }
            _logger.LogWarning("No gramsPerUnit found for count-based ingredient.");
            return 0;
        } 
        private decimal CalculateNutrient(decimal grams, decimal nutrientPer100g)
        {
            return (grams / 100) * nutrientPer100g;
        }
        private NutritionValues SumNutrients(List<IngredientConversion> conversions)
        {
            var total = new NutritionValues();
            
            foreach( var item in conversions)
            {
                total.Calories += item.Calories;
                total.Proteins += item.Proteins;
                total.Carbohydrates += item.Carbohydrates;
                total.Fats += item.Fats;
                if (item.Fibers.HasValue)
                {
                    total.Fibers = (total.Fibers ?? 0) + item.Fibers.Value;
                }
                if (item.Sodium.HasValue)
                {
                    total.Sodium = (total.Sodium ?? 0) + item.Sodium.Value;
                }
            }
            return total;
        }
    }
}
