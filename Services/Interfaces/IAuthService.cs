using synkrone.Model.DTO;

namespace synkrone.Services.Interfaces;

public interface IAuthService
{
    Task<string> GenerateJwtTokenAsync(UserDto user);
}