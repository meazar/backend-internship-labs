using CustomerSupport.API.DTOs;
using CustomerSupport.API.Repositories;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepo;

    public MessageService(IMessageRepository messageRepo) => _messageRepo = messageRepo;

    public async Task<IEnumerable<TicketMessageDto>> GetMessagesForTicketAsync(int ticketId)
    {
        var messages = await _messageRepo.GetMessageByTicket(ticketId);
        return messages.Select(m => new TicketMessageDto(
            m.Id,
            m.TicketId,
            m.AuthorId,
            m.Content,
            m.CreatedAt
        ));
    }
}
