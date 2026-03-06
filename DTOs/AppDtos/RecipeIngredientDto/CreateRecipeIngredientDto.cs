using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.DTOs.AppDtos.RecipeIngredientDto
{
    public class CreateRecipeIngredientDto
    {
        public int IngredientId { get; set; }
        public int UnitId { get; set; }
        public string? Notes { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Quantity { get; set; }
    }
}
