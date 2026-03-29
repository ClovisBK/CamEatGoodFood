namespace AuthService.DTOs.AppDtos.Ratings
{
    public class RatingDetailDto
    {
        public int Score { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
