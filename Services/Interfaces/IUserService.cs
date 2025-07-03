using synkrone.Model.DTO;

namespace synkrone.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserAsync(Guid userId);
}