using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models.AppModels
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public int UnitId { get; set; }
        public string? Notes { get; set; }

        [Column(TypeName ="decimal(10, 2)")]
        public decimal Quantity { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe? Recipe { get; set; }

        [ForeignKey("IngredientId")]
        public Ingredient? Ingredient { get; set; }

        [ForeignKey("UnitId")]
        public IngredientUnit? Unit { get; set; }

    }
}
