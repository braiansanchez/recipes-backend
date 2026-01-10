using Recipes.API.Models;

namespace Recipes.Infrastructure.Interfaces;

public interface IRecipeService
{
    Task<IEnumerable<RecipeReadDto>> GetAllRecipesAsync();
    Task<RecipeReadDto?> GetRecipeByIdAsync(int id);
    Task<RecipeReadDto> CreateRecipeAsync(RecipeCreateDto recipeCreateDto, int userId);
    Task<RecipeReadDto?> UpdateRecipeAsync(int id, RecipeUpdateDto recipeUpdateDto);
    Task<bool> DeleteRecipeAsync(int id);
}
