using System.Security.Claims;
using System.Security.Cryptography.Pkcs;
using System.Text.Json;
using System.Text.Json.Serialization;
using AuthService.Data;
using AuthService.DTOs.AppDtos.Category;
using AuthService.DTOs.AppDtos.Ingredient;
using AuthService.DTOs.AppDtos.Instruction;
using AuthService.DTOs.AppDtos.Nutrition;
using AuthService.DTOs.AppDtos.Recipe;
using AuthService.DTOs.AppDtos.RecipeDto;
using AuthService.DTOs.AppDtos.RecipeIngredientDto;
using AuthService.DTOs.AppDtos.RecipeInstructionDto;
using AuthService.DTOs.AppDtos.User;
using AuthService.Models.AppModels;
using AuthService.Services.Implementations;
using AuthService.Services.Interfaces;
using AuthService.Services.Nutrition;
using AuthService.Services.Nutrition.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RecipesController> _logger;
        private readonly INutritionCalculator _nutritionCalculator;
        private readonly IBlobStorageService _blobService;

        public RecipesController(
            AppDbContext context,
            ILogger<RecipesController> logger,
            INutritionCalculator nutrition,
            IBlobStorageService blobService
            )
        {
            _context = context;
            _logger = logger;
            _nutritionCalculator = nutrition;
            _blobService = blobService;
        }

        [HttpGet("Get-recipes")]
        public async Task<IActionResult> GetRecipes(
            [FromQuery] int limit = 10, 
            [FromQuery] int? cursor = null,
            [FromQuery] int? categoryId = null
            )
        {
            
            if (limit < 1) limit = 10;
            if(limit > 50) limit = 50;

            var query = _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.CreatedBy)
                .OrderByDescending(r => r.Id)
                .AsQueryable();
            if (categoryId.HasValue)
            {
                query = query.Where(r => r.CategoryId == categoryId.Value);
            }
            //applying cursor. Get recipes with ID less than the cursor
            if (cursor.HasValue)
            {
                query = query.Where(r => r.Id < cursor.Value);
            }
            var recipes = await query
                .Take(limit + 1)
                .Select(recipe => new RecipeListResponseDto
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    RegionOrOrigin = recipe.RegionOfOrigin,
                    CreatedAt = recipe.CreatedAt,
                    ImageUrl = recipe.ImageUrl,
                    CategoryName = recipe.Category!.Name,
                    CreatedBy = recipe.CreatedBy!.FirstName,
                    LikeCount = recipe.LikeCount,
                    AverageRating = recipe.AverageRating
                })
                .ToListAsync();

            bool hasMore = recipes.Count > limit;
            if (hasMore)
            {
                recipes.RemoveAt(recipes.Count - 1);
            }
            int? nextCursor = recipes.Any() ? recipes.Last().Id : (int?)null;

            return Ok(new
            {
                recipes,
                nextCursor,
                hasMore,
                count = recipes.Count
            });
        }

        [HttpGet("latest-recipes")]
        public async Task<IActionResult> GetLatestRecipes()
        {
            var recipes = await _context.Recipes
                .Include(c => c.Category)
                .Include(cb => cb.CreatedBy)
                .OrderByDescending(c => c.CreatedAt)
                .Select(r => new RecipeListResponseDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    CreatedAt = r.CreatedAt,
                    LikeCount = r.LikeCount,
                    CategoryName = r.Category!.Name,
                    CreatedBy = r.CreatedBy!.FirstName,
                    AverageRating = r.AverageRating,
                    ImageUrl = r.ImageUrl
                })
                .ToListAsync();

            return Ok(recipes);
                

        }


        /// <summary>
        /// Retrieving a recipe by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A recipe with its details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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

            bool userLiked = false;
            if(!string.IsNullOrEmpty(userId))
            {
                userLiked = await _context.RecipeLikes.AnyAsync(rl => rl.RecipeId == id && rl.UserId == userId);
            }

            var userRatingEntity = await _context.RecipeRatings
                .FirstOrDefaultAsync(rr => rr.RecipeId == id && rr.UserId == userId);

            var userRating = userRatingEntity?.Score;
            

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
                LikeCount = recipe.LikeCount,
                UserLiked = userLiked,
                AverageRating = recipe.AverageRating,
                RatingCount = recipe.RatingCount,
                UserRating = userRating,
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

        [HttpGet("Search-recipe")]
        public  async Task<IActionResult> SearchRecipes(string term)
        {
            var searchTerm = term.Trim().ToLower();
            var recipes = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.CreatedBy)
                .Include(r => r.RecipeIngredients)
                .Where(r => r.Name.ToLower().Contains(searchTerm) ||
                r.RecipeIngredients.Any(ri => ri.Ingredient != null &&
                ri.Ingredient.Name.ToLower().Contains(searchTerm)))
                .Select(r => new RecipeListResponseDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    CategoryName = r.Category!.Name,
                    CreatedBy = r.CreatedBy!.FirstName,
                    CreatedAt = r.CreatedAt,
                    RegionOrOrigin = r.RegionOfOrigin
                })
                .ToListAsync();
                    
            return Ok(recipes);
        }
        /// <summary>
        /// Creating recipe alongside the ingredients and instructions by a contributor
        /// </summary>
        /// <remarks>
        /// **The following are the operations here:**
        /// * Retrieving the authenticated user and verifying for authorization
        /// * Validating the required input fields to ensure they are input
        /// * Extrating the recipes and verifying availability before adding
        /// * Adding instruction fields to the intructions section in the model
        /// * Adding the recipe fields themselves accordingly
        /// </remarks>
        /// <param name="dto">A DTO for the creation of the recipe</param>
        /// <returns>A recipe response body</returns>
        [HttpPost("Add-recipe")]
        public async Task<IActionResult> AddRecipe([FromForm] CreateRecipeDto dto)
        {
            string imageUrl = string.Empty;
            string videoUrl = string.Empty;

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

            var rawIngredients = Request.Form["RecipeIngredient"].ToString();

            if (!string.IsNullOrEmpty(rawIngredients))
            {
                try
                {
                    var joinArray = $"[{rawIngredients}]";
                    var parsed = JsonSerializer.Deserialize<List<CreateRecipeIngredientDto>>(
                        joinArray,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if(parsed != null && parsed.Any())
                    {
                        var ingredientIds = parsed.Select(ri => ri.IngredientId).ToList();

                        var existingIngredients = await _context.Ingredients
                            .Where(i => ingredientIds.Contains(i.Id))
                            .Select(i => i.Id)
                            .ToListAsync();

                        var missingIngredientIds = ingredientIds.Except(existingIngredients).ToList();
                        if (missingIngredientIds.Any())
                            return BadRequest(new { message = "Some ingredients do not exist" });

                        var unitIds = parsed.Select(ri => ri.UnitId).ToList();
                        var existingUnits = await _context.IngredientUnits
                            .Where(u => unitIds.Contains(u.Id))
                            .Select(u => u.Id)
                            .ToListAsync();

                        var missingUnitIds = unitIds.Except(existingUnits).ToList();
                        if (missingUnitIds.Any())
                            return BadRequest(new { message = "Some units do not exist" });

                        foreach(var item in parsed)
                        {
                            if (item.Quantity <= 0)
                                return BadRequest(new { message = "Qauntity must be greater than 0" });
                            recipeIngredients.Add(new RecipeIngredient
                            {
                                IngredientId = item.IngredientId,
                                Quantity = item.Quantity,
                                UnitId = item.UnitId,
                                Notes = item.Notes
                            });
                        }
                    }
                }
                catch (JsonException)
                {
                    return BadRequest(new { message = "Invalid RecipeIngredient format." });
                }
            }
            
            var instructions = new List<RecipeInstruction>();

            var rawInstructions = Request.Form["RecipeInstruction"].ToString();

            if (!string.IsNullOrEmpty(rawInstructions))
            {
                try
                {
                    var jsonArray = $"[{rawInstructions}]";
                    var parsed = JsonSerializer.Deserialize<List<CreateRecipeInstructionDto>>(
                        jsonArray,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if(parsed != null && parsed.Any())
                    {
                        foreach(var item in parsed)
                        {
                            if (string.IsNullOrWhiteSpace(item.ActualInstruction))
                                return BadRequest(new { message = $"Instruction cannot be empty for step {item.StepNumber}" });

                            instructions.Add(new RecipeInstruction
                            {
                                StepNumber = item.StepNumber,
                                ActualInstruction = item.ActualInstruction,
                                EstimatedMinutes = item.EstimatedMinutes
                            });
                        }
                    }
                }
                catch (JsonException)
                {
                    return BadRequest(new { message = "Invalid RecipeInstruction format" });
                }
            }
            
        
            try
            {
                if(dto.ImageFile != null)
                {
                    imageUrl = await _blobService.UploadFileAsync(dto.ImageFile, "image");
                }
                
                if(dto.VideoFile != null)
                {
                    videoUrl = await _blobService.UploadFileAsync(dto.VideoFile, "video");
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }



            var recipe = new Recipe
            {
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                PrepTimeMinutes = dto.PrepTimeMinutes,
                CookTimeMinutes = dto.CookTimeMinutes,
                Servings = dto.Servings,
                ImageUrl = imageUrl,
                VideoUrl = videoUrl,
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
        /// * Retrieving the authenticated user
        /// * Retrieving the recipe of interest by ID
        /// * Verifying the authenticated user owns the recipe to be updated
        /// * Verfying if the recipe of the searched Id exists
        /// * Modifying all fields as needed by the author
        /// </remarks>
        /// <param name="id">The Id of the recipe in In question</param>
        /// <param name="dto">The DTO for updating a recipe with the various fields</param>
        /// <returns>A Message of success</returns>
        [HttpPut("/update-recipe{id}")]
        public async Task<IActionResult> UpdateRecipe(int id,[FromForm] UpdateRecipeDto dto)
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
            recipe.RegionOfOrigin = dto.RegionOfOrigin;
            recipe.LastUpdatedAt = DateTime.UtcNow;

            //handling the files
            try
            {
                if(dto.ImageFile != null)
                {
                    if (!string.IsNullOrEmpty(recipe.ImageUrl))
                        await _blobService.DeleteFileAsync(recipe.ImageUrl);
                    recipe.ImageUrl = await _blobService.UploadFileAsync(dto.ImageFile, "image");
                }
                if(dto.VideoFile != null)
                {
                    if (!string.IsNullOrEmpty(recipe.VideoUrl))
                        await _blobService.DeleteFileAsync(recipe.VideoUrl);
                    recipe.VideoUrl = await _blobService.UploadFileAsync(dto.VideoFile, "video");
                }
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            var rawIngredients = Request.Form["RecipeIngredient"].ToString();
            if (!string.IsNullOrEmpty(rawIngredients))
            {
                try
                {
                    var jsonArray = $"[{rawIngredients}]";
                    var parsed = JsonSerializer.Deserialize<List<CreateRecipeIngredientDto>>(
                        jsonArray,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );
                    if(parsed != null && parsed.Any())
                    {
                        var ingredientIds = parsed.Select(ri => ri.IngredientId).ToList();
                        var existingIngredients = await _context.Ingredients
                            .Where(i => ingredientIds.Contains(i.Id))
                            .Select(i => i.Id)
                            .ToListAsync();

                        var missingIngredientIds = ingredientIds.Except(existingIngredients).ToList();
                        if (missingIngredientIds.Any())
                            return BadRequest(new { message = "Some Ingredients do not exist" });

                        var unitIds = parsed.Select(u => u.UnitId).ToList();

                        var existingUnits = await _context.IngredientUnits
                            .Where(u => unitIds.Contains(u.Id))
                            .Select(u => u.Id)
                            .ToListAsync();

                        var missingUnitIds = unitIds.Except(existingUnits).ToList();
                        if (missingUnitIds.Any())
                            return BadRequest(new { message = "Some units do not exist" });

                        foreach(var item in parsed)
                        {
                            if (item.Quantity <= 0)
                                return BadRequest(new { message = $"Quantity must be greater than 0 for ingredient Id {item.IngredientId}" });
                        }

                        _context.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);

                        var newIngredients = parsed.Select(item => new RecipeIngredient
                        {
                            RecipeId = recipe.Id,
                            IngredientId = item.IngredientId,
                            Quantity = item.Quantity,
                            UnitId = item.UnitId,
                            Notes = item.Notes
                        }).ToList();

                        await _context.RecipeIngredients.AddRangeAsync(newIngredients);

                    }
                }
                catch (JsonException)
                {
                    return BadRequest(new { message = "Invalid RecipeIngredient format" });
                }
            }


            var rawInstruction = Request.Form["RecipeInstruction"].ToString();
            if (!string.IsNullOrEmpty(rawInstruction))
            {
                try
                {
                    var jsonArray = $"[{rawInstruction}]";
                    var parsed = JsonSerializer.Deserialize<List<CreateRecipeInstructionDto>>(
                        jsonArray,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if(parsed != null && parsed.Any())
                    {
                        foreach(var item in parsed)
                        {
                            if (string.IsNullOrWhiteSpace(item.ActualInstruction))
                                return BadRequest(new { message = $"Instruction cannnot be empty for step {item.StepNumber}" });
                        }

                        _context.RecipeInstructions.RemoveRange(recipe.Instructions);

                        var newInstructions = parsed.Select(item => new RecipeInstruction
                        {
                            RecipeId = recipe.Id,
                            StepNumber = item.StepNumber,
                            ActualInstruction = item.ActualInstruction,
                            EstimatedMinutes = item.EstimatedMinutes
                        }).ToList();
                        await _context.RecipeInstructions.AddRangeAsync(newInstructions);
                    }
                }
                catch (JsonException)
                {
                    return BadRequest(new { message = "Invalid RecipeInstruction format." });
                }
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

        [HttpDelete("/delete-recipe/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Please log in to delete a recipe");

            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Instructions)
                .FirstOrDefaultAsync(r => r.Id == id);

            var recipeLikes = await _context.RecipeLikes
                .Where(r => r.RecipeId == id).ToListAsync();

            var recipeRatings = await _context.RecipeRatings
                .Where(rr => rr.RecipeId == id).ToListAsync();

            var recipeComments = await _context.RecipeComments
                .Where(rc => rc.RecipeId == id).ToListAsync();
            

            if (recipe == null)
                return NotFound(new { message = $"No recipe with ID {id} found!" });

            if (recipe.UserId != userId)
                return Forbid("You can only delete your own recipe");

            if (!string.IsNullOrEmpty(recipe.ImageUrl))
            {
                await _blobService.DeleteFileAsync(recipe.ImageUrl);
            }
            if(!string.IsNullOrEmpty(recipe.VideoUrl))
            {
                await _blobService.DeleteFileAsync(recipe.VideoUrl);
            }

            _context.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);
            _context.RecipeInstructions.RemoveRange(recipe.Instructions);
            _context.RecipeLikes.RemoveRange(recipeLikes);
            _context.RecipeRatings.RemoveRange(recipeRatings);
            _context.RecipeComments.RemoveRange(recipeComments);
            _context.Recipes.Remove(recipe);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Recipe deleted successfully" });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to delete recipe ID: {RecipeId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting this recipe" });
            }
        }
    }
}
