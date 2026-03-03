namespace AuthService.DTOs
{
    public class RegisterDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Bio { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Location { get; set; }
        public string? Phone { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
        public string? ProfilePictureUrl { get; set; }
       
        
    }
}
