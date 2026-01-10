using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.API.Models;
using Recipes.Infrastructure.Interfaces;
using System.Security.Claims;

namespace Recipes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto dto, [FromQuery] string adminCode)
    {
        try
        {
            var token = await _authService.RegisterAsync(dto, adminCode);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var token = await _authService.LoginAsync(dto);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> Update(UserUpdateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _authService.UpdateUserAsync(userId, dto);

        return result ? Ok("Perfil actualizado") : BadRequest("No se pudo actualizar");
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleRequestDto request)
    {
        if (string.IsNullOrEmpty(request.googleToken))
            return BadRequest("El token de Google es requerido.");

        try
        {
            var authResponse = await _authService.GoogleLoginAsync(request.googleToken);

            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) 
            return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);

        var success = await _authService.SoftDeleteUserAsync(userId);
        return success ? Ok() : BadRequest();
    }
}

