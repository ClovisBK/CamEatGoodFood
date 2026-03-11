using System.Security.Claims;
using AuthService.Data;
using AuthService.DTOs.AppDtos.Likes;
using AuthService.Models.AppModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{
    [Route("api/recipes/{recipeId}/likes")]
    [ApiController]
    public class RecipeLikesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RecipeLikesController> _logger;

        public RecipeLikesController(AppDbContext context, ILogger<RecipeLikesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleLike(int recipeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "You must be logged in to like a recipe" });

            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
                return NotFound(new { message = $"Recipe with Id {recipeId} not found" });
            
            var existingLike = await _context.RecipeLikes
                .FirstOrDefaultAsync(rl => rl.RecipeId == recipeId && rl.UserId == userId);

            if(existingLike == null)
            {
                var newLike = new RecipeLike
                {
                    UserId = userId,
                    RecipeId = recipeId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.RecipeLikes.Add(newLike);
                recipe.LikeCount++; //Increment counter for likes in recipe
                _logger.LogInformation("User {UserId} liked recipe {RecipeId}", userId, recipeId);
            }
            else
            {
                _context.RecipeLikes.Remove(existingLike);
                recipe.LikeCount--; // Decrement counter on recipe

                _logger.LogInformation("User {UserId} unliked recipe {RecipeId}", userId, recipeId);
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new LikeResponseDto
                {
                    LikeCount = recipe.LikeCount,
                    UserLiked = existingLike == null
                });
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while toggling like for recipe {RecipeId}", recipeId);
                return StatusCode(500, new { message = "An error occurred while processing your like" });
            }
        }

        /// <summary>
        /// Get like status for current user and total count for likes of that recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpGet("status")]
        public async Task<IActionResult> GetLikeStatus(int recipeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("You have to be authenticated");

            var recipe = await _context.Recipes.FindAsync(recipeId);
            if(recipe == null)
                return NotFound();

            var userLiked = await _context.RecipeLikes.AnyAsync(l => l.UserId == userId && l.RecipeId == recipeId);

            return Ok(new LikeResponseDto
            {
                LikeCount = recipe.LikeCount,
                UserLiked = userLiked,
            });
        }
    }
}
