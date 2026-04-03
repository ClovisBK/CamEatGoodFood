using AuthService.Models.AppModels;

namespace AuthService.Repositories.Interfaces
{
    public interface IRecipeRepository
    {
        Task<Recipe?> GetByIdAsync(int id);
        void Update(Recipe recipe);
    }
}
