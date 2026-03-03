using AuthService.Models;

namespace AuthService.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(ApplicationUser user, IList<string> roles);
        RefreshToken GenerateRefreshToken();
    }
}
