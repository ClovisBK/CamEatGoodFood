using AuthService.DTOs.AppDtos.Ingredient;
using AuthService.DTOs.AppDtos.Instruction;
using AuthService.DTOs.AppDtos.Nutrition;

namespace AuthService.DTOs.AppDtos.RecipeDto
{
    public class CreateRecipeResponseDto : RecipeResponseDto
    {
        public List<InstructionDto> Instructions { get; set; } = new();
        public List<IngredientDto> Ingredients { get; set; } = new();
        public NutritionDto Nutrition { get; set; } = null!;
    }
}
