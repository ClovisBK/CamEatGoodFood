using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.AppModels
{
    public class IngredientUnit
    {
         public int Id { get; set; }
        
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;


        [StringLength(20)]
        public string Type { get; set; } = string.Empty;

        [StringLength(10)]
        public string Abbreviation { get; set; } = string.Empty;

        public bool IsBaseUnit { get; set; }
    }
}
