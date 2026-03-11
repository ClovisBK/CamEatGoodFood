using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models.AppModels
{
    public class RecipeLike
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public required string UserId { get; set; }

        public DateTime CreatedAt { get; set; }


        [ForeignKey("RecipeId")]
        public Recipe? Recipe { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
