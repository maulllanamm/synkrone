using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using synkrone.Configuration;
using synkrone.Data;
using synkrone.Model.DTO;
using synkrone.Model.Entities;
using synkrone.Services.Interfaces;

namespace synkrone.Services.Implementations;

public class AuthService: IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly JwtConfig _jwtConfig;
        
    public AuthService(ApplicationDbContext context,  IOptions<JwtConfig> jwtConfig)
    {
        _context = context;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        _logger.LogInformation("Attempting to register user with username: {Username}, email: {Email}",
            registerDto.Username, registerDto.Email);

        // Check if user exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == registerDto.Username || u.Email == registerDto.Email);

        if (existingUser != null)
        {
            _logger.LogWarning("Registration failed: Username or email already exists. Username: {Username}, Email: {Email}",
                registerDto.Username, registerDto.Email);
            throw new ArgumentException("Username or email already exists");
        }

        // Create new user
        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            DisplayName = registerDto.DisplayName ?? registerDto.Username,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            LastSeen = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("New user registered successfully. UserId: {UserId}", user.Id);

        // Manual mapping to UserDto
        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            DisplayName = user.DisplayName,
            ProfilePicture = user.ProfilePicture,
            Bio = user.Bio,
            IsOnline = user.IsOnline,
            LastSeen = user.LastSeen,
            CreatedAt = user.CreatedAt
        };

        var token = await GenerateJwtTokenAsync(userDto);

        _logger.LogInformation("JWT token generated for userId: {UserId}", user.Id);

        return new AuthResponseDto
        {
            Token = token,
            User = userDto,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationInMinutes)
        };
    }
    
    public async Task<string> GenerateJwtTokenAsync(UserDto user)
    {
        _logger.LogInformation("Generating JWT token for userId: {UserId}, username: {Username}", user.Id, user.Username);

        try
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("displayName", user.DisplayName ?? user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationInMinutes),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("JWT token generated successfully for userId: {UserId}", user.Id);

            return await Task.FromResult(tokenString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate JWT token for userId: {UserId}", user.Id);
            throw;
        }
    }

}