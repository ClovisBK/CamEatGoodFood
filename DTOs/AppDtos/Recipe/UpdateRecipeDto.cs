using AuthService.DTOs.AppDtos.RecipeIngredientDto;
using AuthService.DTOs.AppDtos.RecipeInstructionDto;

namespace AuthService.DTOs.AppDtos.Recipe
{
    public class UpdateRecipeDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int? CookTimeMinutes { get; set; }
        public int Servings {  get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? RegionOfOrigin { get; set; }

        public ICollection<CreateRecipeIngredientDto>? RecipeIngredient { get; set; }
        public ICollection<CreateRecipeInstructionDto>? RecipeInstruction { get; set; }
    }
}
