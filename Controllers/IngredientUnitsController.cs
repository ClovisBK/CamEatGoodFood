using AuthService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientUnitsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IngredientUnitsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Get-ingredient-units")]
        public async Task<IActionResult> GetIngredientUnits()
        {
            var units = await _context.IngredientUnits.ToListAsync();
            return Ok(units);
        }
    }
}
