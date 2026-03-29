namespace AuthService.DTOs.AppDtos.Ratings
{
    public class RatingDistributionDto
    {
        public int FiveStar { get; set; }
        public int FourStar { get; set; }
        public int ThreeStar { get; set; }
        public int TwoStar { get; set; }
        public int OneStar { get; set; }

        public int TotalCount => FiveStar + FourStar + ThreeStar + TwoStar + OneStar;
    }
}
