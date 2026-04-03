namespace AuthService.DTOs.AppDtos.Ratings
{
    public class ReatingResponseDto
    {
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
        public int? UserRating {  get; set; }
    }
}
