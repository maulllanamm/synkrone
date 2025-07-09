namespace synkrone.Model.DTO;

public class MessageReactionDto
{
    public Guid Id { get; set; }
    public string Emoji { get; set; }
    public UserDto User { get; set; }
    public DateTime CreatedAt { get; set; }
}