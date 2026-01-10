using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.Core.Entities;

[Table("Categories")]
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Recipe> Recipes { get; set; } = new();
}

