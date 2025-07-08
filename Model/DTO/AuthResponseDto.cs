namespace synkrone.Model.DTO;

public class AuthResponseDto
{
    public string Token { get; set; }
    public UserDto User { get; set; }
    public DateTime ExpiresAt { get; set; }
}