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

}