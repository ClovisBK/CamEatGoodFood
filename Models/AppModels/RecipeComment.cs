using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models.AppModels
{
    public class RecipeComment
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        public int? ParentCommentId   { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }

        public bool isEdited { get; set; }
        public bool isDeleted { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe? Recipe { get; set; }

        [ForeignKey("ParentCommentId")]
        public RecipeComment? ParentComment { get; set; }

        public ICollection<RecipeComment> Replies { get; set; } = new List<RecipeComment>();

    }
}
