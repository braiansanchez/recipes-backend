namespace Recipes.API.Models;

public class RecipeCreateDto
{
    public required string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CookingTimeMinutes { get; set; }
    public string? ImageUrl { get; set; }
    public List<IngredientCreateDto> Ingredients { get; set; } = new();
}
