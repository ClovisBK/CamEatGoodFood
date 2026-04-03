using AuthService.Models.AppModels;

namespace AuthService.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<RecipeRating?> GetUserRatingAsync(int recipe, string userId);
        Task<IEnumerable<RecipeRating>> GetRecipeRatingAsync(int recipeId);
        Task<IEnumerable<int>> GetRecipeScoresAsync(int recipeId);
        Task AddAsync(RecipeRating rating);
        void Update(RecipeRating rating);
        void Delete(int recipeId);
    }
}
