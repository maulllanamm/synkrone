using Microsoft.EntityFrameworkCore;
using synkrone.Data;
using synkrone.Model.DTO;
using synkrone.Services.Interfaces;

namespace synkrone.Services.Implementations;

public class UserService: IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;
    public UserService(ApplicationDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<UserDto?> GetByIdAsync(Guid userId)
    {
        _logger.LogDebug("Fetching user from database: {UserId}", userId);
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            _logger.LogInformation("No user found with ID: {UserId}", userId);
            return null;
        }

        _logger.LogInformation("User retrieved: {@User}", user);
        return new UserDto
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
    }

    public async Task<List<UserDto>> SearchUsersAsync(string searchTerm)
    {
        _logger.LogDebug("Fetching user from database: {searchTerm}", searchTerm);
        var users = await _context.Users
            .Where(u => u.Username.Contains(searchTerm) || 
                        u.DisplayName.Contains(searchTerm) || 
                        u.Email.Contains(searchTerm))
            .Take(20)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                DisplayName = u.DisplayName,
                ProfilePicture = u.ProfilePicture,
                Bio = u.Bio,
                IsOnline = u.IsOnline,
                LastSeen = u.LastSeen,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        return users;
    }

    public async Task<UserDto> UpdateProfileAsync(Guid userId, UpdateUserDto updateUserDto)
    {
        _logger.LogInformation("Attempting to update profile for userId: {UserId}", userId);

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", userId);
            throw new ArgumentException("User not found");
        }

        if (!string.IsNullOrEmpty(updateUserDto.DisplayName))
        {
            _logger.LogInformation("Updating DisplayName for userId: {UserId}", userId);
            user.DisplayName = updateUserDto.DisplayName;
        }

        if (!string.IsNullOrEmpty(updateUserDto.Bio))
        {
            _logger.LogInformation("Updating Bio for userId: {UserId}", userId);
            user.Bio = updateUserDto.Bio;
        }

        if (!string.IsNullOrEmpty(updateUserDto.ProfilePicture))
        {
            _logger.LogInformation("Updating ProfilePicture for userId: {UserId}", userId);
            user.ProfilePicture = updateUserDto.ProfilePicture;
        }

        user.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated user profile for userId: {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save updated profile for userId: {UserId}", userId);
            throw;
        }

        return new UserDto
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
    }

}