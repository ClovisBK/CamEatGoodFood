using Microsoft.AspNetCore.Identity;

namespace AuthService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
