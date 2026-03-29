using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.AppDtos.Comments
{
    public class CreateCommentDto
    {
        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;
        public int? ParentCommentId { get; set; }
    }
}
