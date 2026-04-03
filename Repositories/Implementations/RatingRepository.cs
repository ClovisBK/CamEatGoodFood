using AuthService.Data;
using AuthService.Models.AppModels;
using AuthService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories.Implementations
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;
        public RatingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<RecipeRating?> GetUserRatingAsync(int recipeId, string userId)
            => await _context.RecipeRatings
                    .FirstOrDefaultAsync(rr => rr.RecipeId == recipeId && rr.UserId == userId);
        
        public async Task<IEnumerable<RecipeRating>> GetRecipeRatingAsync(int recipeId)
            => await _context.RecipeRatings
                    .Where(rr => rr.RecipeId == recipeId)
                    .ToListAsync();

        public async Task<IEnumerable<int>> GetRecipeScoresAsync(int recipeId)
            => await _context.RecipeRatings
                .Where(rr => rr.RecipeId == recipeId)
                .Select(rr => rr.Score)
                .ToListAsync();
        public async Task AddAsync(RecipeRating rating) 
            => await _context.RecipeRatings.AddAsync(rating);

        public void Update(RecipeRating rating)
            => _context.RecipeRatings.Update(rating);
        public void Delete(RecipeRating rating)
            => _context.RecipeRatings.Remove(rating);



    }
}
