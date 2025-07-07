using synkrone.Model.DTO;

namespace synkrone.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(Guid userId);
    Task<List<UserDto>> SearchUsersAsync(string searchTerm);
}