namespace Recipes.API.Models;

public class RecipeReadDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<IngredientDto> Ingredients { get; set; } = new();
    public string? ImageUrl { get; set; }
    public int UserId { get; set; }
}
