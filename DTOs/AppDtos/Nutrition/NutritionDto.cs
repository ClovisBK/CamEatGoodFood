namespace AuthService.DTOs.AppDtos.Nutrition
{
    public class NutritionDto
    {
        public decimal Calories { get; set; }
        public decimal Proteins { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Fats { get; set; }
        public decimal? Fibers { get; set; }
        public decimal? Sodium { get; set; }
    }
}
