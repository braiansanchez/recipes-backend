namespace Recipes.API.Models;

public class IngredientCreateDto
{
    public required string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Unit { get; set; } = string.Empty;
}
