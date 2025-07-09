using Microsoft.AspNetCore.Mvc;
using synkrone.Model.DTO;
using synkrone.Services.Interfaces;

namespace synkrone.Controllers;

public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<UserController> _logger;
    
    public AuthController(ILogger<UserController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        _logger.LogInformation("Register endpoint called for username: {Username}", registerDto.Username);

        try
        {
            var result = await _authService.RegisterAsync(registerDto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Registration failed for username: {Username}", registerDto.Username);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during registration");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}