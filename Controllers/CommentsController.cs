using System.Security.Claims;
using AuthService.Data;
using AuthService.DTOs.AppDtos.Comments;
using AuthService.Models.AppModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

namespace AuthService.Controllers
{
    [Route("api/recipes/{recipeId}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(AppDbContext context, ILogger<CommentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
                return NotFound(new { message = "Recipe not found" });

            var comments = await _context.RecipeComments
                .Include(c => c.User)
                .Where(c => c.RecipeId == recipeId && c.ParentCommentId == null && !c.isDeleted)
                .OrderByDescending(c => c.Id)
                .Select(c => new
                {
                    c.Id,
                    c.Content,
                    userName = $"{c.User!.FirstName} {c.User.LastName}".Trim(),
                    c.CreatedAt,
                    c.isEdited,
                    c.isDeleted,
                    Replies = _context.RecipeComments
                    .Include(r => r.User)
                    .Where(r => r.ParentCommentId == c.Id && !r.isDeleted)
                    .OrderBy(r => r.CreatedAt)
                    .Select(r => new
                    {
                        r.Id,
                        r.Content,
                        userName = $"{r.User!.FirstName} {r.User.LastName}".Trim(),
                        r.CreatedAt,
                        r.isEdited
                    })
                    .ToList()
                })
                .ToListAsync();
            return Ok(new
            {
                recipeId,
                commentCount = comments.Count,
                comments
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(int recipeId, [FromBody] CreateCommentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("please login to comment");

            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest(new { message = "Comment content cannot be empty" });

            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
                return NotFound("Recipe not found");
            int? effectiveParentId = dto.ParentCommentId > 0 ? dto.ParentCommentId : null;
            if (effectiveParentId.HasValue)
            {
                var parentComment = await _context.RecipeComments
                    .FirstOrDefaultAsync(rc => rc.Id == dto.ParentCommentId && rc.RecipeId ==  recipeId);

                if (parentComment == null)
                    return BadRequest(new { message = "Parent comment not found" });
            }

           
            
            var comment = new RecipeComment
            {
                RecipeId = recipeId,
                UserId = userId,
                Content = dto.Content,
                ParentCommentId = effectiveParentId,
                CreatedAt = DateTime.UtcNow,
                isEdited = false,
                isDeleted = false
            };

            _context.RecipeComments.Add(comment);
            recipe.CommentCount++;

            try
            {
                await _context.SaveChangesAsync();

                await _context.Entry(comment).Reference(c => c.User).LoadAsync();

                var response = new CommentDto
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    UserId = userId,
                    UserName = $"{comment.User!.FirstName} {comment.User.LastName}".Trim(),
                    CreatedAt = DateTime.UtcNow,
                    IsEdited = comment.isEdited
                };

                return Ok(new
                {
                    message = "Comment added successfully",
                    comment = response
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding comment to recipe {RecipeId}", recipeId);
                return StatusCode(500,new { message = "An error occurred while adding the comment your comment" });
            }
        }
        /// <summary>
        /// Editing a comment by the comment writer
        /// </summary>
        /// <param name="recipeId">The recipe to which the comment belongs</param>
        /// <param name="commentId">The comment that is being edited</param>
        /// <param name="dto">The update block with the necessary fields for updating the comment</param>
        /// <returns>A response body of the edited comment with the new comment, time, and status of edit</returns>
        [HttpPut("{commentId}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int recipeId, int commentId, [FromBody] UpdateCommentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Please log in to update this comment");

            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest(new { message = "Comment content cannot be empty" });

            var comment = await _context.RecipeComments
                .FirstOrDefaultAsync(rc => rc.Id == commentId && rc.RecipeId == recipeId);

            if (comment == null)
                return NotFound("Comment not found");
            if (comment.UserId != userId)
                return Forbid("You can only edit your own comment");

            if (comment.isDeleted)
                return BadRequest("You can't edit deleted comment");

            comment.Content = dto.Content;
            comment.LastUpdatedAt = DateTime.UtcNow;
            comment.isEdited = true;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Comment has been successfully edited",
                    comment = new
                    {
                        comment.Id,
                        comment.Content,
                        comment.isEdited,
                        comment.LastUpdatedAt
                    }
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating comment {CommentId}", commentId);
                return StatusCode(500, new { message = "an error occurred while updating your comment" });
            }
        }
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int recipeId, int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();


            var comment = await _context.RecipeComments
                .FirstOrDefaultAsync(c => c.Id == commentId && c.RecipeId == recipeId);
            if (comment == null)
                return NotFound(new { message = "Comment not found" });

            if (comment.UserId != userId)
                return Forbid("You can only delete your own comment");
            if (comment.isDeleted)
                return BadRequest("Comment is already deleted");

            comment.isDeleted = true;
            comment.Content = "[Comment deleted]";

            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe != null)
            {
                recipe.CommentCount--;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Comment successfully deleted" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error  deleting comment {CommentId}", commentId);
                return StatusCode(500, new { message = "An error occurred while deleting your comment" });
            }
        }

        
        
       
    }
}
