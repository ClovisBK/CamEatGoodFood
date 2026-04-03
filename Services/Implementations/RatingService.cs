using AuthService.DTOs.AppDtos.Ratings;
using AuthService.Models.AppModels;
using AuthService.Repositories.Interfaces;
using AuthService.Services.Interfaces;

namespace AuthService.Services.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<RatingService> _logger;

        public RatingService(IUnitOfWork uow, ILogger<RatingService> logger)
        {
            _logger = logger;
            _uow = uow;
        }
        public async Task<RatingResponseDto?> RateRecipeAsync(int recipeId, string userId, int score)
        {
            var recipe = await _uow.Recipes.GetByIdAsync(recipeId);
            if (recipe is null) return null;
            
            var existingRating = await _uow.Ratings.GetUserRatingAsync(recipeId, userId);
            if (existingRating is null)
            {
                var newRating = new RecipeRating
                {
                    UserId = userId,
                    Score = score,
                    RecipeId = recipeId,
                    CreatedAt = DateTime.UtcNow
                };
                await _uow.Ratings.AddAsync(newRating);
                _logger.LogInformation("User rated recipe {RecipeId} with {Score} stars", recipeId, score);
            }
            else
            {
                existingRating.Score = score;
                existingRating.LastUpdatedAt = DateTime.UtcNow;
                _uow.Ratings.Update(existingRating);
                _logger.LogInformation("User updated recipe {RecipeId} with {Score} stars", recipeId, score);
            }
            await RecalculateAndUpdateRecipeAsync(recipeId, recipe);

            await _uow.CompleteAsync();

            return new RatingResponseDto
            {
                AverageRating = recipe.AverageRating,
                RatingCount = recipe.RatingCount,
                UserRating = score
            };
        }
        public async Task<bool> DeleteRatingAsync(int recipeId, string userId)
        {
            var recipe = await _uow.Recipes.GetByIdAsync(recipeId);
            if (recipe is null) return false;
            
            var existingRating = await _uow.Ratings.GetUserRatingAsync(recipeId, userId);
            if(existingRating is null) return false;

            _uow.Ratings.Delete(existingRating);

            await RecalculateAndUpdateRecipeAsync(recipeId, recipe);

            await _uow.CompleteAsync();

            _logger.LogInformation("User removed their rating for recipe {RecipeId}", recipeId);
            return true;
        }

        public async Task<RatingDistributionDto?> GetRatingDistributionAsync(int recipeId)
        {
            var recipe = await _uow.Recipes.GetByIdAsync(recipeId);
            if (recipe is null) return null;

            var ratings = await _uow.Ratings.GetRecipeRatingAsync(recipeId);

            return new RatingDistributionDto
            {
                FiveStar = ratings.Count(r => r.Score == 5),
                FourStar = ratings.Count(r => r.Score == 4),
                ThreeStar = ratings.Count(r => r.Score == 3),
                TwoStar = ratings.Count(r => r.Score == 2),
                OneStar = ratings.Count(r => r.Score == 1)
            };

        }

        public async Task<RatingResponseDto?> GetRatingStatusAsync(int recipeId, string? userId)
        {
            var recipe = await _uow.Recipes.GetByIdAsync(recipeId);
            if(recipe is null) return null;

            int? userRating = null;
            if (!string.IsNullOrEmpty(userId))
            {
                var userRatingEntity = await _uow.Ratings.GetUserRatingAsync(recipeId, userId);
                userRating = userRatingEntity?.Score;
            }
            return new RatingResponseDto
            {
                AverageRating = recipe.AverageRating,
                RatingCount = recipe.RatingCount,
                UserRating = userRating,
            };
        }

        private async Task RecalculateAndUpdateRecipeAsync(int recipeId, Recipe recipe)
        {
            var scores = await _uow.Ratings.GetRecipeScoresAsync(recipeId);
            var scoreList = scores.ToList();

            recipe.RatingCount = scoreList.Count;
            recipe.AverageRating = scoreList.Any() ? Math.Round(scoreList.Average(), 2) : 0;

            _uow.Recipes.Update(recipe);
        }
    }
}
