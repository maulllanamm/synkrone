using synkrone.Model.DTO;

namespace synkrone.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<string> GenerateJwtTokenAsync(UserDto user);
    Task<UserDto?> GetUserByIdAsync(Guid userId);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
}