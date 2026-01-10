using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.Core.Entities;

[Table("Recipes")]
public class Recipe
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Ingredient> Ingredients { get; set; } = new();
    public string Instructions { get; set; } = string.Empty;
    public int CookingTimeMinutes { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPrivate { get; set; } = false;
    public int Servings { get; set; } // Porciones
    public string Difficulty { get; set; } = "Media";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public List<Category> Categories { get; set; } = new();
}
