using System.Security.Claims;
using AuthService.Data;
using AuthService.DTOs.AppDtos.Ratings;
using AuthService.Models.AppModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{
    [Route("api/recipes/{recipeId}/ratings")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RatingsController> _logger;

        public RatingsController(AppDbContext context, ILogger<RatingsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RateRecipe(int recipeId, [FromQuery] int score)
        {
            if (score < 1 || score > 5)
                return BadRequest(new { message = "Rating must be between 1 and 5 stars" });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("You must be logged in to rate this recipe");

            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
                return NotFound($"Recipe with ID {recipeId} not found");

            var existingRating = await _context.RecipeRatings
                .FirstOrDefaultAsync(rr => rr.RecipeId == recipeId && rr.UserId ==  userId);

            bool isNewRating = existingRating == null;
            if (isNewRating)
            {
                var newRating = new RecipeRating
                { 
                    UserId= userId,
                    Score = score,
                    RecipeId = recipeId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.RecipeRatings.Add(newRating);

                _logger.LogInformation("User {UserId} rated recipe {RecipeId} with {Score} stars", userId, recipeId, score);
            }
            else
            {
                existingRating!.Score = score;
                existingRating.LastUpdatedAt = DateTime.UtcNow;

                _context.RecipeRatings.Update(existingRating);
                _logger.LogInformation("User {UserId} updated their rating for recipe {RecipeId} with {Score} stars", userId, recipeId, score);

            }

            try
            {
                await _context.SaveChangesAsync();

                await RecalculateRecipeRatings(recipeId);

                await _context.SaveChangesAsync();

                var updateRecipe = await _context.Recipes.FindAsync(recipeId);
                return Ok(new ReatingResponseDto
                {
                    AverageRating = updateRecipe!.AverageRating,
                    RatingCount = updateRecipe.RatingCount,
                    UserRating = score,
                });
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while rating recipe {RecipeId}", recipeId);
                return StatusCode(500, new { message = "An error occurred while saving your rating" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRating(int recipeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("You have to be logged in first");

            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
                return NotFound();

            var existingRating = await _context.RecipeRatings
                .FirstOrDefaultAsync(rr => rr.RecipeId == recipeId && rr.UserId == userId);
            if (existingRating == null)
                return NotFound(new { message = "You haven't rated this recipe" });

            _context.RecipeRatings.Remove(existingRating);


            await _context.SaveChangesAsync();

            await RecalculateRecipeRatings(recipeId);

            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} removed their rating for recipe {RecipeId}", userId, recipeId);

            return Ok(new { message = "Rating removed successfully" });
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetRatingStatus(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if(recipe == null) return NotFound();

            int? userRating = null;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var userRatingEntity = await _context.RecipeRatings
                    .FirstOrDefaultAsync(rr => rr.RecipeId == recipeId && rr.UserId == userId);

                userRating = userRatingEntity?.Score;
            }
            return Ok(new ReatingResponseDto
            {
                AverageRating = recipe.AverageRating,
                UserRating = userRating,
                RatingCount = recipe.RatingCount
            });
        }

        [HttpGet("distribution")]
        public async Task<IActionResult> GetRatingDistribution(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if(recipe == null) return NotFound();

            var ratings = await _context.RecipeRatings
                .Where(rr => rr.RecipeId == recipeId)
                .ToListAsync();
            var distribution = new RatingDistributionDto
            {
                FiveStar = ratings.Count(r => r.Score == 5),
                FourStar = ratings.Count(r => r.Score == 4),
                ThreeStar = ratings.Count(r => r.Score == 3),
                TwoStar = ratings.Count(r => r.Score == 2),
                OneStar = ratings.Count(r => r.Score == 1)
            };

            return Ok(distribution);
        }

        private async Task RecalculateRecipeRatings(int recipeId)
        {
            var ratings = await _context.RecipeRatings
                .Where(rr => rr.RecipeId == recipeId)
                .Select(rr => rr.Score)
                .ToListAsync();

            var recipe = await _context.Recipes.FindAsync(recipeId);
            if(recipe != null)
            {
                if (ratings.Any())
                {
                    recipe.RatingCount = ratings.Count;
                    recipe.AverageRating = Math.Round(ratings.Average(), 2);
                }
                else
                {
                    recipe.RatingCount = 0;
                    recipe.AverageRating = 0;
                }
            }
        }
    }
}
