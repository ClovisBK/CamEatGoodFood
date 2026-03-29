using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.AppDtos.Comments
{
    public class UpdateCommentDto
    {
        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;
    }
}
