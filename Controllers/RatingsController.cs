using System.Security.Claims;
using AuthService.Data;
using AuthService.DTOs.AppDtos.Ratings;
using AuthService.Models.AppModels;
using AuthService.Services.Interfaces;
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
        private readonly IRatingService _ratingService;
        private readonly ILogger<RatingsController> _logger;

        public RatingsController(IRatingService ratingService, ILogger<RatingsController> logger)
        {
            _ratingService = ratingService;
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


                var result = await _ratingService.RateRecipeAsync(recipeId, userId, score);
                if (result == null)
                    return NotFound($"RecipeId with ID {recipeId} not found");
                return Ok(result);
            }

            [HttpDelete]
            public async Task<IActionResult> DeleteRating(int recipeId)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("You have to be logged in first");

                var deleted = await _ratingService.DeleteRatingAsync(recipeId, userId);
                if (!deleted)
                    return NotFound();
                return Ok(new { message = "Rating successfully removed" });
            }

            [HttpGet("status")]
            public async Task<IActionResult> GetRatingStatus(int recipeId)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _ratingService.GetRatingStatusAsync(recipeId, userId);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }

            [HttpGet("distribution")]
            public async Task<IActionResult> GetRatingDistribution(int recipeId)
            {
                var result = await _ratingService.GetRatingDistributionAsync(recipeId);
                if (result == null)
                    return NotFound("No rating was found");
                return Ok(result);
            }
    }
}
