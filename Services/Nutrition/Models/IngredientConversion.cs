namespace AuthService.Services.Nutrition.Models
{
    public class IngredientConversion
    {
        public string IngredientName { get; set; } = string.Empty;
        public decimal Grams { get; set; }
        public decimal Calories { get; set; }
        public decimal Proteins { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Fats { get; set; }
        public decimal? Fibers { get; set; }
        public decimal? Sodium { get; set; }

    }
}
