using synkrone.Model.DTO;

namespace synkrone.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(Guid userId);
    Task<List<UserDto>> SearchUsersAsync(string searchTerm);
    Task<UserDto> UpdateProfileAsync(Guid userId, UpdateUserDto updateUserDto);
    Task<bool> UpdateOnlineStatusAsync(Guid userId, bool isOnline);
}