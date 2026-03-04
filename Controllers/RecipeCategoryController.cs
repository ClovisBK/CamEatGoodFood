using AuthService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecipeCategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-recipe-categories")]
        public async Task<IActionResult> GetRecipeCategories()
        {
            var categories = await _context.RecipeCategories.ToListAsync();
            return Ok(categories);
        }
    }
}
