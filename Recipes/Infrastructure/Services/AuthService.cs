using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Recipes.API.Models;
using Recipes.Core.Entities;
using Recipes.Infrastructure.Data;
using Recipes.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Recipes.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string> RegisterAsync(UserRegisterDto dto, string adminCode)
    {
        var secretFromConfig = _config["AdminSettings:RegistrationCode"];
        if (adminCode != secretFromConfig)
            throw new Exception("Código de invitación inválido.");

        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            throw new Exception("El email ya está registrado");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return GenerateJwtToken(user);
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            throw new Exception("Usuario no encontrado");

        if (string.IsNullOrEmpty(user.PasswordHash))
            throw new Exception("Esta cuenta utiliza Google. Por favor, inicia sesión con el botón de Google.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Credenciales incorrectas");

        if (!user.IsActive || user.IsDeleted)
            throw new Exception("Cuenta desactivada o eliminada.");

        return GenerateJwtToken(user);
    }

    public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto dto)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        user.Username = dto.Username ?? user.Username;

        if (!string.IsNullOrEmpty(dto.NewPassword))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        return await _context.SaveChangesAsync() > 0;
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AuthResponseDto> GoogleLoginAsync(string googleToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _config["Google:ClientId"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken, settings);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

            if (user == null)
            {
                // nuevo usuario
                user = new User
                {
                    Email = payload.Email,
                    Username = payload.Name,
                    PasswordHash = null,
                    IsActive = true,
                    Role = "User",
                    CreatedAt = DateTime.UtcNow,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return new AuthResponseDto
            {
                Token = GenerateJwtToken(user),
                UserId = user.Id,
                Username = user.Username
            };
        }
        catch (InvalidJwtException)
        {
            throw new Exception("El token de Google no es válido o ha expirado.");
        }
        catch (Exception ex)
        {
            throw new Exception("Error en la autenticación con Google: " + ex.Message);
        }
    }

    public async Task<bool> SoftDeleteUserAsync(int userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted)
                return false;

            user.IsDeleted = true;

            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Error interno al intentar eliminar la cuenta.");
        }
    }
}

