using AuthService.Data;
using AuthService.Models.AppModels;
using AuthService.Repositories.Interfaces;

namespace AuthService.Repositories.Implementations
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly AppDbContext _context;
        public RecipeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Recipe?> GetByIdAsync(int id)
            => await _context.Recipes.FindAsync(id);

        public void Update(Recipe recipe)
            => _context.Recipes.Update(recipe);
    }
}
