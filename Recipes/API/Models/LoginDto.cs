namespace Recipes.API.Models;

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserRegisterDto : LoginDto 
{
    public string Username { get; set; } = string.Empty;
}

public class UserUpdateDto
{
    public string Password { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? NewPassword { get; set; }
}