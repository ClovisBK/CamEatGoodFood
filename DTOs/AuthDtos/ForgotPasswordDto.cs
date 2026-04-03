using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.AuthDtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
