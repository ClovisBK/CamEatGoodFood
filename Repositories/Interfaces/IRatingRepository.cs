using AuthService.Models.AppModels;

namespace AuthService.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<RecipeRating?> GetUserRatingAsync(int recipeId, string userId);
        Task<IEnumerable<RecipeRating>> GetRecipeRatingAsync(int recipeId);
        Task<IEnumerable<int>> GetRecipeScoresAsync(int recipeId);
        Task AddAsync(RecipeRating rating);
        void Update(RecipeRating rating);
        void Delete(RecipeRating recipeId);
    }
}
