using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models.AppModels
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public required string UserId { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        
        public int PrepTimeMinutes { get; set; }
        public int? CookTimeMinutes { get; set; }
        public int Servings { get; set; } // this is what will determine distribution of nutrients accordinly
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? RegionOfOrigin { get; set; }
        public int LikeCount { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? CreatedBy { get; set; }

        [ForeignKey("CategoryId")]
        public RecipeCategory? Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }


        // Navigation Properties
        public ICollection<RecipeInstruction> Instructions { get; set; } = new List<RecipeInstruction>();
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public ICollection<RecipeLike> RecipeLikes { get; set; } = new List<RecipeLike>();

    }
}
