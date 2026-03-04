namespace AuthService.Models.AppModels
{
    public class RecipeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
