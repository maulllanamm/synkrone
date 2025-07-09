using synkrone.Data;
using synkrone.Model.DTO;
using synkrone.Model.Entities;
using synkrone.Services.Interfaces;

namespace synkrone.Services.Implementations;

public class ChatService: IChatService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ChatService> _logger;
    
    public ChatService(ApplicationDbContext context, ILogger<ChatService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<MessageDto> SendMessageAsync(Guid senderId, SendMessageDto messageDto)
    {
        _logger.LogInformation("User {SenderId} is sending a message", senderId);

        if (messageDto.GroupId.HasValue && messageDto.ReceiverId.HasValue)
        {
            _logger.LogWarning("Invalid message: both ReceiverId and GroupId filled. Sender: {SenderId}", senderId);
            throw new ArgumentException("Message cannot be both group and private");
        }

        if (!messageDto.GroupId.HasValue && !messageDto.ReceiverId.HasValue)
        {
            _logger.LogWarning("Invalid message: neither ReceiverId nor GroupId filled. Sender: {SenderId}", senderId);
            throw new ArgumentException("Message must specify either group or receiver");
        }


        var message = new Message
        {
            Content = messageDto.Content,
            Type = messageDto.Type,
            AttachmentUrl = messageDto.AttachmentUrl,
            SenderId = senderId,
            ReceiverId = messageDto.ReceiverId,
            GroupId = messageDto.GroupId,
            ReplyToMessageId = messageDto.ReplyToMessageId,
            SentAt = DateTime.UtcNow,
            IsEdited = false,
            IsDeleted = false
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        await _context.Entry(message).Reference(m => m.Sender).LoadAsync();

        if (message.ReplyToMessageId.HasValue)
        {
            await _context.Entry(message).Reference(m => m.ReplyToMessage).LoadAsync();
        }

        _logger.LogInformation("Message {MessageId} sent by user {SenderId}", message.Id, senderId);

        return new MessageDto
        {
            Id = message.Id,
            Content = message.Content,
            Type = message.Type,
            AttachmentUrl = message.AttachmentUrl,
            SentAt = message.SentAt,
            IsEdited = message.IsEdited,
            EditedAt = message.EditedAt,
            ReceiverId = message.ReceiverId,
            GroupId = message.GroupId,
            ReplyToMessageId = message.ReplyToMessageId,
            Reactions = new List<MessageReactionDto>(), 
            Sender = new UserDto
            {
                Id = message.Sender.Id,
                Username = message.Sender.Username,
                Email = message.Sender.Email,
                DisplayName = message.Sender.DisplayName,
                ProfilePicture = message.Sender.ProfilePicture,
                Bio = message.Sender.Bio,
                IsOnline = message.Sender.IsOnline,
                LastSeen = message.Sender.LastSeen,
                CreatedAt = message.Sender.CreatedAt
            }
        };
    }
    
}