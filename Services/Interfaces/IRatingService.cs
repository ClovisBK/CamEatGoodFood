using AuthService.DTOs.AppDtos.Ratings;

namespace AuthService.Services.Interfaces
{
    public interface IRatingService
    {
        Task<RatingResponseDto?> RateRecipeAsync(int recipe, string userId, int score);
        Task<bool> DeleteRatingAsync(int recipeId, string userId);
        Task<RatingResponseDto?> GetRatingStatusAsync(int recipeId, string? userId);
        Task<RatingDistributionDto?> GetRatingDistributionAsync(int recipeId);
    }
}
