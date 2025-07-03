using synkrone.Data;
using synkrone.Model.DTO;
using synkrone.Services.Interfaces;

namespace synkrone.Services.Implementations;

public class UserService: IUserService
{
    private readonly ApplicationDbContext _context;
    
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserDto?> GetUserAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null) return null;

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