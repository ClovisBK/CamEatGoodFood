using AuthService.DTOs.AppDtos.Category;
using AuthService.DTOs.AppDtos.User;

namespace AuthService.DTOs.AppDtos.RecipeDto
{
    public class RecipeResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int? CookTimeMinutes { get; set; }
        public int Servings { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? RegionOrOrigin { get; set;}
        public CategoryDto Category { get; set; } = null!;
        public UserDto CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated {  get; set; }

        
    }
}
