namespace Recipes.Core.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Recipe> Recipes { get; set; } = new();
}

