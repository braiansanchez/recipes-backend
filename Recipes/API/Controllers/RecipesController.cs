using Microsoft.AspNetCore.Mvc;
using Recipes.API.Models;
using Recipes.Core.Entities;
using Recipes.Infrastructure.Interfaces;

namespace Recipes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipesController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetAll()
    {
        return Ok(await _recipeService.GetAllRecipesAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Recipe>> GetById(int id)
    {
        var recipe = await _recipeService.GetRecipeByIdAsync(id);

        if (recipe is null)
            return NotFound();

        return Ok(recipe);
    }

    [HttpPost]
    public async Task<ActionResult<RecipeReadDto>> Create(RecipeCreateDto recipeCreateDto)
    {
        var result = await _recipeService.CreateRecipeAsync(recipeCreateDto);

        // Devuelve un estado 201 y la URL para consultar el nuevo recurso
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, RecipeUpdateDto recipeUpdateDto)
    {
        var updatedRecipe = await _recipeService.UpdateRecipeAsync(id, recipeUpdateDto);

        if (updatedRecipe == null)
            return NotFound();

        return Ok(updatedRecipe);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _recipeService.DeleteRecipeAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}

