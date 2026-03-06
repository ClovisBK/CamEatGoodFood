using System.Security.Claims;
using System.Security.Cryptography.Pkcs;
using AuthService.Data;
using AuthService.DTOs.AppDtos.Category;
using AuthService.DTOs.AppDtos.Ingredient;
using AuthService.DTOs.AppDtos.Instruction;
using AuthService.DTOs.AppDtos.Nutrition;
using AuthService.DTOs.AppDtos.Recipe;
using AuthService.DTOs.AppDtos.RecipeDto;
using AuthService.DTOs.AppDtos.User;
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

            var response = new List<RecipeListResponseDto>();

            foreach(var recipe in recipes)
            {
                var nutrition = _nutritionCalculator.CalculateRecipeNutrition(recipe.RecipeIngredients, recipe.Servings);

                var recipeDto = new RecipeListResponseDto
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    Description = recipe.Description,
                    PrepTimeMinutes = recipe.PrepTimeMinutes,
                    CookTimeMinutes = recipe.CookTimeMinutes,
                    Servings = recipe.Servings,
                    ImageUrl = recipe.ImageUrl,
                    VideoUrl = recipe.VideoUrl,
                    RegionOrOrigin = recipe.RegionOfOrigin,
                    CreatedAt = recipe.CreatedAt,
                    Category = new CategoryDto
                    {
                        Id = recipe.Category!.Id,
                        Name = recipe.Category!.Name,
                        Description = recipe.Description
                    },
                    CreatedBy = new UserDto
                    {
                       
                        Name = recipe.CreatedBy!.FirstName,
                        Email = recipe.CreatedBy.Email ?? string.Empty,
                        FullName = $"{recipe.CreatedBy.FirstName} {recipe.CreatedBy.LastName}".Trim()

                    },
                    Instructions = recipe.Instructions
                    .OrderBy(i => i.StepNumber)
                    .Select(i => new InstructionDto
                    {
                        StepNumber = i.StepNumber,
                        Instruction = i.ActualInstruction,
                        EstimatedMinutes = i.EstimatedMinutes
                    }).ToList(),

                    Ingredients = recipe.RecipeIngredients.Select(ri => new IngredientDto
                    {
                        IngredientId = ri.IngredientId,
                        IngredientName = ri.Ingredient!.Name,
                        Quantity = ri.Quantity,
                        Unit = ri.Unit!.Name,
                        Notes = ri.Notes
                    }).ToList(),
                    Nutrition = new NutritionDto
                    {
                        Calories = nutrition.PerServing.Calories,
                        Proteins = nutrition.PerServing.Proteins,
                        Carbohydrates = nutrition.PerServing.Carbohydrates,
                        Fats = nutrition.PerServing.Fats,
                        Fibers = nutrition.PerServing.Fibers,
                        Sodium = nutrition.PerServing.Sodium,
                    }

                };
                response.Add(recipeDto);
            }

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

            var response = new RecipeDetailsResponseDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                PrepTimeMinutes = recipe.PrepTimeMinutes,
                CookTimeMinutes = recipe.CookTimeMinutes,
                Servings = recipe.Servings,
                ImageUrl = recipe.ImageUrl,
                VideoUrl = recipe.VideoUrl,
                RegionOrOrigin = recipe.RegionOfOrigin,

                Category = new CategoryDto
                {
                    Id = recipe.Category!.Id,
                    Name = recipe.Category.Name
                },
                CreatedBy = new UserDto
                {
                    
                    Name = recipe.CreatedBy!.FirstName,
                    Email = recipe.CreatedBy.Email ?? string.Empty,
                    FullName = $"{recipe.CreatedBy.FirstName} {recipe.CreatedBy.LastName}".Trim()
                },
                CreatedAt = recipe.CreatedAt,
                Instructions = recipe.Instructions.OrderBy(i => i.StepNumber).Select(i => new InstructionDto
                {
                    StepNumber = i.StepNumber,
                    Instruction = i.ActualInstruction,
                    EstimatedMinutes = i.EstimatedMinutes
                }).ToList(),
                Ingredients = recipe.RecipeIngredients.Select(ri => new IngredientDto
                {
                    IngredientId = ri.IngredientId,
                    IngredientName = ri.Ingredient!.Name,
                    Quantity = ri.Quantity,
                    Unit = ri.Unit!.Name,
                    Notes = ri.Notes
                }).ToList(),
                Nutrition = new NutritionDto
                {
                    Calories = nutritionResult.PerServing.Calories,
                    Proteins = nutritionResult.PerServing.Proteins,
                    Carbohydrates = nutritionResult.PerServing.Carbohydrates,
                    Fats = nutritionResult.PerServing.Fats,
                    Fibers = nutritionResult.PerServing?.Fibers,
                    Sodium = nutritionResult.PerServing?.Sodium
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

                var nutrition = _nutritionCalculator.CalculateRecipeNutrition(createdRecipe!.RecipeIngredients, createdRecipe.Servings);

                var response = new CreateRecipeResponseDto
                {
                    Id = createdRecipe!.Id,
                    Name = createdRecipe.Name,
                    Description = createdRecipe.Description,
                    Category = new CategoryDto
                    {
                        Id = createdRecipe.Category!.Id,
                        Name = createdRecipe.Category.Name,
                        Description = createdRecipe.Category.Description,
                    },
                    PrepTimeMinutes = createdRecipe.PrepTimeMinutes,
                    CookTimeMinutes = createdRecipe.CookTimeMinutes,
                    Servings = createdRecipe.Servings,
                    ImageUrl = createdRecipe.ImageUrl,
                    VideoUrl = createdRecipe.VideoUrl,
                    RegionOrOrigin = createdRecipe.RegionOfOrigin,
                    CreatedBy = new UserDto
                    {
                        Name = createdRecipe.CreatedBy!.FirstName,
                        FullName = $"{createdRecipe.CreatedBy.FirstName} {createdRecipe.CreatedBy.LastName}".Trim(),
                        Email = createdRecipe.CreatedBy.Email ?? string.Empty
                    },
                    CreatedAt = createdRecipe.CreatedAt,
                    Instructions = createdRecipe.Instructions.OrderBy(i => i.StepNumber).Select(i => new InstructionDto
                    {
                        StepNumber = i.StepNumber,
                        Instruction = i.ActualInstruction,
                        EstimatedMinutes = i.EstimatedMinutes
                    }).ToList(),
                    Ingredients = createdRecipe.RecipeIngredients.Select(ri => new IngredientDto
                    {
                        IngredientId = ri.IngredientId,
                        IngredientName = ri.Ingredient!.Name,
                        Quantity = ri.Quantity,
                        Unit = ri.Unit!.Name,
                        Notes = ri.Notes

                    }).ToList(),

                    Nutrition = new NutritionDto
                    {
                        Calories = nutrition.PerServing.Calories,
                        Proteins = nutrition.PerServing.Proteins,
                        Carbohydrates = nutrition.PerServing.Carbohydrates,
                        Fats = nutrition.PerServing.Fats,
                        Fibers = nutrition.PerServing.Fibers,
                        Sodium = nutrition.PerServing.Sodium
                    }

                };

                return CreatedAtAction(nameof(GetRecipeById), new { id = recipe.Id }, createdRecipe);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to create recipe: {recipeName}", dto.Name);
                return StatusCode(500, new { message = "An error occurred while creating the recipe" });
            }

        }
        /// <summary>
        /// Updating the recipe alongside ingredients and instructions
        /// </summary>
        /// <remarks>
        /// **We are doing the following:**
        /// *Retrieving the authenticated user
        /// *Retrieving the recipe of interest by ID
        /// *Verifying the authenticated user owns the recipe to be updated
        /// *Verfying if the recipe of the searched Id exists
        /// *Modifying all fields as needed by the author
        /// </remarks>
        /// <param name="id">The Id of the recipe in In question</param>
        /// <param name="dto">The DTO for updating a recipe with the various fields</param>
        /// <returns>A Message of success</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, UpdateRecipeDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Not authorized here");

            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Instructions)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
                return NotFound(new { message = $"Recipe with ID {id} not found" });

            if (recipe.UserId != userId)
                return Forbid("You can only update your own recipes");

            recipe.Name = dto.Name;
            recipe.Description = dto.Description;
            recipe.CategoryId = dto.CategoryId;
            recipe.PrepTimeMinutes = dto.PrepTimeMinutes;
            recipe.CookTimeMinutes = dto.CookTimeMinutes;
            recipe.Servings = dto.Servings;
            recipe.ImageUrl = dto.ImageUrl;
            recipe.VideoUrl = dto.VideoUrl;
            recipe.RegionOfOrigin = dto.RegionOfOrigin;
            recipe.LastUpdatedAt = DateTime.UtcNow;

            if(dto.RecipeIngredient != null)
            {
                _context.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);

                var newIngredients = dto.RecipeIngredient.Select(item => new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    IngredientId = item.IngredientId,
                    Quantity = item.Quantity,
                    UnitId = item.UnitId,
                    Notes = item.Notes
                }).ToList();

                await _context.RecipeIngredients.AddRangeAsync(newIngredients);
            }
            if(dto.RecipeInstruction  != null)
            {
                _context.RecipeInstructions.RemoveRange(recipe.Instructions);

                var newInstructions = dto.RecipeInstruction.Select(item => new RecipeInstruction
                {
                    RecipeId = recipe.Id,
                    StepNumber = item.StepNumber,
                    ActualInstruction = item.ActualInstruction,
                    EstimatedMinutes = item.EstimatedMinutes
                }).ToList();
                await _context.RecipeInstructions.AddRangeAsync(newInstructions);
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Updated successfully" });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to update recipe ID: {RecipeId}", id);
                return StatusCode(500, new { message = "An error occurred while updating the recipe" });
            }

        }
    }
}
