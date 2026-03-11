using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models.AppModels
{
    public class RecipeRating
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public required string UserId { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe? Recipe { get; set; }

    }
}
