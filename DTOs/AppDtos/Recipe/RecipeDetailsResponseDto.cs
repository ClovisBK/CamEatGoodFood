using AuthService.DTOs.AppDtos.Ingredient;
using AuthService.DTOs.AppDtos.Instruction;
using AuthService.DTOs.AppDtos.Nutrition;
using AuthService.DTOs.AppDtos.Ratings;

namespace AuthService.DTOs.AppDtos.RecipeDto
{
    public class RecipeDetailsResponseDto : RecipeResponseDto
    {
        public int LikeCount { get; set; }
        public bool UserLiked { get; set; }
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
        public int? UserRating {  get; set; }
        public List<InstructionDto> Instructions { get; set; } = new();
        public List<IngredientDto> Ingredients { get; set;} = new();
        public NutritionDto Nutrition { get; set; } = null!;
        public RatingDistributionDto? RatingDistribution {  get; set; }
    }
}
