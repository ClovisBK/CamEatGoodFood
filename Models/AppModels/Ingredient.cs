using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models.AppModels
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DefaultUnitId { get; set; }

        [Range(0.01, 10000.00)]
        [Column(TypeName ="decimal(10,2)")]
        public decimal CaloriesPer100g { get; set; }
        [Range(0, 100)]
        [Column(TypeName ="decimal(7,2)")]
        public decimal Proteins { get; set; }

        [Range(0, 100)]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Carbohydrates { get; set; }

        [Range(0, 100)]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Fats { get; set; }

        [Range(0, 100)]
        [Column(TypeName = "decimal(7,2)")]
        public decimal? Fibers { get; set; }

        [Range(0, 100)]
        [Column(TypeName = "decimal(7,2)")]
        public decimal? Sodium { get; set; }

        [Range(0, 1000.00)]
        [Column(TypeName = "decimal(6,2)")]
        public decimal? GramsPerUnit { get; set; }

        [Range(0, 100)]
        [Column(TypeName = "decimal(3,2)")]
        public decimal? Density { get; set; }
        public IngredientUnit? IngredientUnit { get; set; }

    }
}
