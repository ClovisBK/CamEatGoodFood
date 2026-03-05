using System.Security.Claims;
using AuthService.Data;
using AuthService.DTOs.AppDtos.RecipeDto;
using AuthService.Models.AppModels;
using AuthService.Services.Nutrition;
using AuthService.Services.Nutrition.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RecipiesController> _logger;
        private readonly INutritionCalculator _nutritionCalculator;

        public RecipiesController(AppDbContext context, ILogger<RecipiesController> logger, INutritionCalculator nutrition)
        {
            _context = context;
            _logger = logger;
            _nutritionCalculator = nutrition;
        }

        [HttpGet("Get-recipes")]
        public async Task<IActionResult> GetRecipes()
        {
            var recipes = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.CreatedBy)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Unit)
                .Include(r => r.Instructions)
                .ToListAsync();

            var response = recipes.Select(r => new
            {
                id = r.Id,
                name = r.Name,
                description = r.Description,
                prepTimeMinutes = r.PrepTimeMinutes,
                cookTimeMinutes = r.CookTimeMinutes,
                servings = r.Servings,
                imageUrl = r.ImageUrl,
                videoUrl = r.VideoUrl,
                category = new
                {
                    id = r.Category!.Id,
                    name = r.Category.Name
                },
                createdBy = new
                {
                    id = r.CreatedBy!.Id,
                    fullName = $"{r.CreatedBy.FirstName} {r.CreatedBy.LastName}".Trim(),
                  
                },
                createdAt = r.CreatedAt,
                instructions = r.Instructions.OrderBy(i => i.StepNumber).Select(i => new
                {
                    stepNumber = i.StepNumber,
                    instruction = i.ActualInstruction,
                    estimatedMinutes = i.EstimatedMinutes
                }),
                ingredients = r.RecipeIngredients.Select(ri => new
                {
                    ingredientName = ri.Ingredient!.Name,
                    quantity = ri.Quantity,
                    unit = ri.Unit!.Name,
                    notes = ri.Notes
                })
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.CreatedBy)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Unit)
                .Include(r => r.Instructions.OrderBy(i => i.StepNumber))
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
                return NotFound(new { message = $"Recipe with ID {id} not found" });

            var nutritionResult = _nutritionCalculator.CalculateRecipeNutrition(recipe.RecipeIngredients, recipe.Servings);

            var response = new
            {
                id = recipe.Id,
                name = recipe.Name,
                description = recipe.Description,
                prepTimeMinutes = recipe.PrepTimeMinutes,
                cookTimeMinutes = recipe.CookTimeMinutes,
                servings = recipe.Servings,
                imageUrl = recipe.ImageUrl,
                videoUrl = recipe.VideoUrl,
                regionOfOrigin = recipe.RegionOfOrigin,
                category = new
                {
                    id = recipe.Category!.Id,
                    name = recipe.Category.Name
                },
                createdBy = new
                {
                    id = recipe.CreatedBy!.Id,
                    name = recipe.CreatedBy.FirstName,
                    email = recipe.CreatedBy.Email
                },
                createdAt = recipe.CreatedAt,
                instructions = recipe.Instructions.OrderBy(i => i.StepNumber).Select(i => new
                {
                    stepNumber = i.StepNumber,
                    instruction = i.ActualInstruction,
                    estimateMinutes = i.EstimatedMinutes
                }),
                ingredients = recipe.RecipeIngredients.Select(ri => new
                {
                    ingredientId = ri.IngredientId,
                    ingredientName = ri.Ingredient!.Name,
                    quantity = ri.Quantity,
                    unit = ri.Unit!.Name,
                    notes = ri.Notes
                }),
                nutrition = new
                {
                    calories = nutritionResult.PerServing.Calories,
                    proteins = nutritionResult.PerServing.Proteins,
                    carbohydrates = nutritionResult.PerServing.Carbohydrates,
                    fats = nutritionResult.PerServing.Fats,
                    fibers = nutritionResult.PerServing?.Fibers,
                    sodium = nutritionResult.PerServing?.Sodium
                }
                

            };
            return Ok(response);
        }

        [HttpPost("Add-recipe")]
        [Authorize(Roles ="Contributor")]
        public async Task<IActionResult> AddRecipe(CreateRecipeDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Not authorized here");

        

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Recipe name is required" });
            if (dto.CategoryId <= 0)
                return BadRequest(new { message = "valid category is required" });
            if (dto.Servings <= 0)
                return BadRequest(new { message = "Servings must be at least 1" });

            var categoryExists = await _context.RecipeCategories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
                return BadRequest(new { message = $"Category Id {dto.CategoryId} does not exist" });

            //processing of the ingredients
            var recipeIngredients = new List<RecipeIngredient>();
            if(dto.RecipeIngredient != null && dto.RecipeIngredient.Any())
            {
                var ingredients = dto.RecipeIngredient.Select(ri => ri.IngredientId).ToList();

                var existingIngredients = await _context.Ingredients
                    .Where(i => ingredients.Contains(i.Id))
                    .Select(i => i.Id)
                    .ToListAsync();

                var missingIngredientIds = ingredients.Except(existingIngredients).ToList();
                if (missingIngredientIds.Any())
                    return BadRequest(new { message = "Some ingredients do not exist" });


                var unitIds = dto.RecipeIngredient.Select(ri => ri.UnitId).ToList();
                
                var exisingUnits = await _context.IngredientUnits
                    .Where(u => unitIds.Contains(u.Id))
                    .Select(u => u.Id)
                    .ToListAsync();

                var missingUnitIds = unitIds.Except(exisingUnits).ToList();
                if (missingUnitIds.Any())
                    return BadRequest(new { message = "Some units do not exist" });

                foreach(var item in dto.RecipeIngredient)
                {
                    if (item.Quantity <= 0)
                        return BadRequest(new { message = $"Quantity must be greater than 0 for ingredient ID {item.IngredientId}" });
                    recipeIngredients.Add(new RecipeIngredient
                    {
                        IngredientId = item.IngredientId,
                        Quantity = item.Quantity,
                        UnitId = item.UnitId,
                        Notes = item.Notes
                    });
                }
                      
            }

            var instructions = new List<RecipeInstruction>();
            
            if(dto.RecipeInstruction != null && dto.RecipeInstruction.Any())
            {
                var stepNumbers = dto.RecipeInstruction.Select(i => i.StepNumber).OrderBy(s => s).ToList();

                

                foreach(var item in dto.RecipeInstruction)
                {
                    if (string.IsNullOrWhiteSpace(item.ActualInstruction))
                        return BadRequest(new { message = $"Instruction cannot be empty for step {item.StepNumber}"});

                    instructions.Add(new RecipeInstruction
                    {
                        StepNumber = item.StepNumber,
                        ActualInstruction = item.ActualInstruction,
                        EstimatedMinutes = item.EstimatedMinutes,
                    });
                }
            }

            // Creating the recipe

            var recipe = new Recipe
            {
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                PrepTimeMinutes = dto.PrepTimeMinutes,
                CookTimeMinutes = dto.CookTimeMinutes,
                Servings = dto.Servings,
                ImageUrl = dto.ImageUrl,
                VideoUrl = dto.VideoUrl,
                RegionOfOrigin = dto.RegionOfOrigin,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                RecipeIngredients = recipeIngredients,
                Instructions = instructions
            };

            // saving to database

            try
            {
                await _context.Recipes.AddAsync(recipe);
                await _context.SaveChangesAsync();

                var createdRecipe = await _context.Recipes
                    .Include(r => r.Category)
                    .Include(r => r.CreatedBy)
                    .Include(r => r.RecipeIngredients)
                      .ThenInclude(ri => ri.Ingredient)
                    .Include(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Unit)
                    .Include(r => r.Instructions)
                    .FirstOrDefaultAsync(r => r.Id == recipe.Id);

                //shaping the created recipe with minimal include

                var response = new
                {
                    id = createdRecipe!.Id,
                    name = createdRecipe.Name,
                    description = createdRecipe.Description,
                    category = new
                    {
                        id = createdRecipe.Category!.Id,
                        name = createdRecipe.Category.Name,
                    },
                    prepTimeMinutes = createdRecipe.PrepTimeMinutes,
                    cookTimeMinutes = createdRecipe.CookTimeMinutes,
                    servings = createdRecipe.Servings,
                    imageUrl = createdRecipe.ImageUrl,
                    videoUrl = createdRecipe.VideoUrl,
                    regionOfOrigin = createdRecipe.RegionOfOrigin,
                    createdBy = new
                    {
                        id = createdRecipe.CreatedBy!.Id,
                        fullName = $"{createdRecipe.CreatedBy.FirstName} {createdRecipe.CreatedBy.LastName}".Trim(),
                        email = createdRecipe.CreatedBy.Email
                    },
                    createdAt = createdRecipe.CreatedAt,
                    instructions = createdRecipe.Instructions.OrderBy(i => i.StepNumber).Select(i => new
                    {
                        stepNumber = i.StepNumber,
                        instruction = i.ActualInstruction,
                        estimatedMinutes = i.EstimatedMinutes
                    }),
                    ingredients = createdRecipe.RecipeIngredients.Select(ri => new
                    {
                        ingredienId = ri.IngredientId,
                        ingredientName = ri.Ingredient!.Name,
                        quantity = ri.Quantity,
                        unit = ri.Unit!.Name,
                        notes = ri.Notes

                    })


                };

                return CreatedAtAction(nameof(GetRecipeById), new { id = recipe.Id }, createdRecipe);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to create recipe: {recipeName}", dto.Name);
                return StatusCode(500, new { message = "An error occurred while creating the recipe" });
            }

        }
    }
}
