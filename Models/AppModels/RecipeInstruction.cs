using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models.AppModels
{
    public class RecipeInstruction
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe? Recipe { get; set; }
        public int StepNumber { get; set; }
        public string ActualInstruction { get; set; } = string.Empty;
        public int EstimatedMinutes { get; set; }
        public bool IsOptional { get; set; } = false;
    }
}
