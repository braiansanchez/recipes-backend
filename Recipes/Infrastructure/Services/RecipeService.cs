using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recipes.API.Models;
using Recipes.Core.Entities;
using Recipes.Infrastructure.Data;
using Recipes.Infrastructure.Interfaces;

namespace Recipes.Infrastructure.Services;

public class RecipeService : IRecipeService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public RecipeService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecipeReadDto>> GetAllRecipesAsync()
    {
        var recipes = await _context.Recipes
            .Include(r => r.Categories)
            .Include(r => r.Ingredients)
            .Include(r => r.User)
            .ToListAsync();

        return _mapper.Map<IEnumerable<RecipeReadDto>>(recipes);
    }

    public async Task<RecipeReadDto?> GetRecipeByIdAsync(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Categories)
            .Include(r => r.Ingredients)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        return _mapper.Map<RecipeReadDto>(recipe);
    }

    public async Task<RecipeReadDto> CreateRecipeAsync(RecipeCreateDto recipeCreateDto)
    {
        //TO DO:se debe mappear tambien Catergories
        var recipeEntity = _mapper.Map<Recipe>(recipeCreateDto);

        _context.Recipes.Add(recipeEntity);
        await _context.SaveChangesAsync();

        return _mapper.Map<RecipeReadDto>(recipeEntity);
    }

    public async Task<RecipeReadDto?> UpdateRecipeAsync(int id, RecipeUpdateDto recipeUpdateDto)
    {
        //TO DO:se debe mappear tambien Catergories
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null) 
            return null;

        recipe.Title = recipeUpdateDto.Title;
        recipe.Description = recipeUpdateDto.Description;
        recipe.ImageUrl = recipeUpdateDto.ImageUrl;

        _context.Ingredients.RemoveRange(recipe.Ingredients);

        _mapper.Map(recipeUpdateDto, recipe);

        await _context.SaveChangesAsync();
        return _mapper.Map<RecipeReadDto>(recipe);
    }

    public async Task<bool> DeleteRecipeAsync(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe is null) 
            return false;

        _context.Recipes.Remove(recipe);
        return await _context.SaveChangesAsync() > 0;
    }
}
