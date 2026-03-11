using AuthService.DTOs.AppDtos.Ingredient;
using AuthService.DTOs.AppDtos.Instruction;
using AuthService.DTOs.AppDtos.Nutrition;

namespace AuthService.DTOs.AppDtos.RecipeDto
{
    public class RecipeDetailsResponseDto : RecipeResponseDto
    {
        public int LikeCount { get; set; }
        public bool UserLiked { get; set; }
        public List<InstructionDto> Instructions { get; set; } = new();
        public List<IngredientDto> Ingredients { get; set;} = new();
        public NutritionDto Nutrition { get; set; } = null!;
    }
}
