using synkrone.Model.DTO;

namespace synkrone.Services.Interfaces;

public interface IChatService
{
    Task<MessageDto> SendMessageAsync(Guid senderId, SendMessageDto messageDto);
}