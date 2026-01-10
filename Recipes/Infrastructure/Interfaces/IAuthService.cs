using Recipes.API.Models;

namespace Recipes.Infrastructure.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(UserRegisterDto dto, string adminCode);
    Task<string> LoginAsync(LoginDto dto);
    Task<bool> UpdateUserAsync(int userId, UserUpdateDto dto);
    Task<AuthResponseDto> GoogleLoginAsync(string googleToken);
    Task<bool> SoftDeleteUserAsync(int userId);
}
