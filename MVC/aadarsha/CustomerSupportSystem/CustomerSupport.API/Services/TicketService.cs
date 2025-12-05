using CustomerSupport.API.DTOs;
using CustomerSupport.API.Models;
using CustomerSupport.API.Repositories;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepo;

    public TicketService(ITicketRepository ticketRepo) => _ticketRepo = ticketRepo;

    public async Task<TicketDto?> GetTicketByIdAsync(int id)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null)
            return null;

        return new TicketDto(
            ticket.Id,
            ticket.TicketNumber,
            ticket.Subject,
            ticket.Description,
            ticket.CustomerId,
            ticket.AssignedAgentId,
            ticket.TeamId,
            ticket.Status,
            ticket.CreatedAt,
            ticket.UpdatedAt
        );
    }

    public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync(int? customerId = null)
    {
        var tickets = await _ticketRepo.GetAllTicketsAsync(customerId);
        return tickets.Select(t => new TicketDto(
            t.Id,
            t.TicketNumber,
            t.Subject,
            t.Description,
            t.CustomerId,
            t.AssignedAgentId,
            t.TeamId,
            t.Status,
            t.CreatedAt,
            t.UpdatedAt
        ));
    }

    public async Task<int> CreateTicketAsync(TicketDto ticketDto)
    {
        var entity = new Ticket
        {
            Subject = ticketDto.Subject ?? string.Empty,
            Description = ticketDto.Description ?? string.Empty,
            CustomerId = ticketDto.CustomerId,
            AssignedAgentId = ticketDto.AssignedAgentId,
            TeamId = ticketDto.TeamId,
        };
        return await _ticketRepo.CreateAsync(entity);
    }

    public async Task AssignAgentToTicketAsync(int ticketId, int agentId)
    {
        await _ticketRepo.AssignAgentAsync(ticketId, agentId);
    }

    public async Task<bool> UpdateTicketStatusAsync(int ticketId, string status)
    {
        return await _ticketRepo.UpdateTicketStatusAsync(ticketId, status);
    }
}
