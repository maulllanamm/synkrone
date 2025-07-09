using System.ComponentModel.DataAnnotations;
using synkrone.Model.Enum;

namespace synkrone.Model.DTO;

public class SendMessageDto
{
    [Required]
    public string Content { get; set; }
        
    public MessageType Type { get; set; } = MessageType.Text;
        
    public string? AttachmentUrl { get; set; }
        
    public Guid? ReceiverId { get; set; }
        
    public Guid? GroupId { get; set; }
        
    public Guid? ReplyToMessageId { get; set; }
}