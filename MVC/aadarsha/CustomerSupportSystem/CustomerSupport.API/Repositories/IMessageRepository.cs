using CustomerSupport.API.Models;

namespace CustomerSupport.API.Repositories;

public interface IMessageRepository
{
    Task<List<TicketMessage>> GetMessageByTicket(int ticketId);
    Task<int> AddMessageAsync(int ticketId, string content, int authorId);
}
