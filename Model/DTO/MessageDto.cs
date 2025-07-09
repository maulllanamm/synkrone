using synkrone.Model.Enum;

namespace synkrone.Model.DTO;

public class MessageDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public MessageType Type { get; set; }
    public string? AttachmentUrl { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsEdited { get; set; }
    public DateTime? EditedAt { get; set; }
    public UserDto Sender { get; set; }
    public Guid? ReceiverId { get; set; }
    public Guid? GroupId { get; set; }
    public Guid? ReplyToMessageId { get; set; }
    public List<MessageReactionDto> Reactions { get; set; } = new();
}