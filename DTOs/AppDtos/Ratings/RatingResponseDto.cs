namespace AuthService.DTOs.AppDtos.Ratings
{
    public class RatingResponseDto
    {
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
        public int? UserRating {  get; set; }
    }
}
