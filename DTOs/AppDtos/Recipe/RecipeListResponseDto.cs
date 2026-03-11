using AuthService.DTOs.AppDtos.Category;
using AuthService.DTOs.AppDtos.Ingredient;
using AuthService.DTOs.AppDtos.Instruction;
using AuthService.DTOs.AppDtos.Nutrition;
using AuthService.DTOs.AppDtos.User;

namespace AuthService.DTOs.AppDtos.RecipeDto
{
    public class RecipeListResponseDto 
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? RegionOrOrigin { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; }
        public int AverageRating { get; set; }
        public int RatingCount { get; set; }
    }
}
