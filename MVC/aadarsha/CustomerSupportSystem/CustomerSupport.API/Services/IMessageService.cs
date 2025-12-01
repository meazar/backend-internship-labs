using CustomerSupport.API.DTOs;

public interface IMessageService
{
    Task<IEnumerable<TicketMessageDto>> GetMessagesForTicketAsync(int ticketId);
}
