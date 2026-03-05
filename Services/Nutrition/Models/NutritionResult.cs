namespace AuthService.Services.Nutrition.Models
{
    public class NutritionResult
    {
        public NutritionValues Total { get; set; } = new();
        public NutritionValues PerServing { get; set; } = new();
    }

    public class NutritionValues
    {
        public decimal Calories { get; set; }
        public decimal Proteins { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Fats { get; set; }
        public decimal? Fibers   { get; set; }
        public decimal? Sodium { get; set; }


        // helper method for rounding values for display
        public void RoundValues()
        {
            Calories = Math.Round(Calories, 0);
            Proteins = Math.Round(Proteins, 1);
            Carbohydrates = Math.Round(Carbohydrates, 1);
            Fats = Math.Round(Fats, 1);
            if(Fibers.HasValue) Fibers = Math.Round(Fibers.Value, 1);
            if(Sodium.HasValue) Sodium = Math.Round(Sodium.Value, 2);
        }
    }
}
