using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.Core.Entities;

[Table("Users")]
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public string Role { get; set; } = "User";

    //auditoria
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }

    public List<Recipe> Recipes { get; set; } = new();
}

