using CustomerSupport.API.Models;

namespace CustomerSupport.API.Repositories;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(int id);
    Task<IEnumerable<Ticket>> GetAllTicketsAsync(int? agentId);
    Task<int> CreateAsync(Ticket ticket);
    Task AssignAgentAsync(int ticketId, int agentId);
    Task<TicketMessage> AddMessageAsync(int ticketId, string content, int authorId);
    Task<bool> UpdateTicketStatusAsync(int ticketId, string status);
    Task<bool> TeamExistsAsync(int teamId);
    Task<bool> CanReassignTeamAsync(int ticketId);
    Task<bool> AssignTeamAsync(int ticketId, int? teamId);
}
