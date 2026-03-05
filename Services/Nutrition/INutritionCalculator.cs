using AuthService.Models.AppModels;
using AuthService.Services.Nutrition.Models;

namespace AuthService.Services.Nutrition
{
    public interface INutritionCalculator
    {
        /// <summary>
        /// Calculates Nutritional information for a recipe
        /// </summary>
        /// <param name="recipeIngredients">Collection of recipe ingredients with ingredient and unit loaded</param>
        /// <param name="servings">Number of servings the recipe yields</param>
        /// <returns>Complete nutrition results with total and per-serving values</returns>
        NutritionResult CalculateRecipeNutrition(ICollection<RecipeIngredient> recipeIngredients, int servings);
    }
}
