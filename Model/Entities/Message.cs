using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using synkrone.Model.Enum;

namespace synkrone.Model.Entities;

public class Message
{
    public Guid Id { get; set; }
        
    [Required]
    public string Content { get; set; }
        
    public MessageType Type { get; set; } = MessageType.Text;
        
    public string? AttachmentUrl { get; set; }
        
    public DateTime SentAt { get; set; }
        
    public bool IsEdited { get; set; }
        
    public DateTime? EditedAt { get; set; }
        
    public bool IsDeleted { get; set; }
        
    public Guid SenderId { get; set; }
    public User Sender { get; set; }
        
    public Guid? ReceiverId { get; set; }
    public User? Receiver { get; set; }
        
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
        
    public Guid? ReplyToMessageId { get; set; }
    public Message? ReplyToMessage { get; set; }
    
}