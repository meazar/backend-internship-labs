using CustomerSupport.API.DTOs;

public interface ITicketService
{
    Task<TicketDto?> GetTicketByIdAsync(int id);
    Task<IEnumerable<TicketDto>> GetAllTicketsAsync(int? customerId = null);
    Task<int> CreateTicketAsync(TicketDto ticketDto);
    Task AssignAgentToTicketAsync(int ticketId, int agentId);
    Task<bool> UpdateTicketStatusAsync(int ticketId, string status);
}
