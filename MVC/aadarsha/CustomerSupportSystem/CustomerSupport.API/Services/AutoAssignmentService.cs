using CustomerSupport.API.Repositories;

namespace CustomerSupport.API.Services;

public class AutoAssignmentService : IAutoAssignmentService
{
    private readonly IAgentRepository _agentRepo;
    private readonly ITicketRepository _ticketRepo;
    private readonly ILogger<AutoAssignmentService> _logger;

    public AutoAssignmentService(
        IAgentRepository agentRepo,
        ITicketRepository ticketRepo,
        ILogger<AutoAssignmentService> logger
    )
    {
        _agentRepo = agentRepo;
        _ticketRepo = ticketRepo;
        _logger = logger;
    }

    public async Task<int?> FindBestAgentAsync(int? teamId = null)
    {
        try
        {
            var availableAgents = await _agentRepo.GetAvailableAgentsAsync(teamId);
            if (!availableAgents.Any())
            {
                _logger.LogWarning("No available agents found for team {TeamId}", teamId);
                return null;
            }

            var bestAgent = availableAgents
                .Where(a => a.IsAvailable && a.CurrentTickets < a.MaxTickets)
                .OrderBy(a => (double)a.CurrentTickets / a.MaxTickets) // Load percentage (0.0 = empty, 1.0 = full)
                .ThenBy(a => a.CurrentTickets)
                .FirstOrDefault();

            if (bestAgent == null)
            {
                _logger.LogWarning("No agents with available capacity found");
                return null;
            }

            _logger.LogInformation(
                "Selected agent {AgentId} with load {Current}/{Max} ({Percentage:P0})",
                bestAgent.Id,
                bestAgent.CurrentTickets,
                bestAgent.MaxTickets,
                (double)bestAgent.CurrentTickets / bestAgent.MaxTickets
            );

            return bestAgent.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding best agent for team {TeamId}", teamId);
            return null;
        }
    }

    public async Task<bool> AutoAssignTicketAsync(int ticketId)
    {
        try
        {
            var ticket = await _ticketRepo.GetByIdAsync(ticketId);
            if (ticket == null)
            {
                _logger.LogError("Ticket {TicketId} not found for auto-assignment", ticketId);
                return false;
            }

            if (ticket.AssignedAgentId.HasValue)
            {
                _logger.LogInformation(
                    "Ticket {TicketId} already assigned to agent {AgentId}",
                    ticketId,
                    ticket.AssignedAgentId
                );
                return true;
            }

            var bestAgentId = await FindBestAgentAsync(ticket.TeamId);

            if (bestAgentId.HasValue)
            {
                await _ticketRepo.AssignAgentAsync(ticketId, bestAgentId.Value);

                await _agentRepo.IncrementTicketCountAsync(bestAgentId.Value);

                _logger.LogInformation(
                    "Auto-assigned ticket {TicketId} to agent {AgentId}",
                    ticketId,
                    bestAgentId.Value
                );

                return true;
            }

            _logger.LogWarning("No available agent found for ticket {TicketId}", ticketId);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error auto-assigning ticket {TicketId}", ticketId);
            return false;
        }
    }
}
