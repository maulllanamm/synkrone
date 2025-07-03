namespace synkrone.Model.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? DisplayName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Bio { get; set; }
    public bool IsOnline { get; set; }
    public DateTime LastSeen { get; set; }
    public DateTime CreatedAt { get; set; }
}