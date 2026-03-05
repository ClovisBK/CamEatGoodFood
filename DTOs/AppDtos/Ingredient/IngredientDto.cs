namespace AuthService.DTOs.AppDtos.Ingredient
{
    public class IngredientDto
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
