using CustomerSupport.API.Models;

namespace CustomerSupport.API.Repositories;

public interface IAgentRepository
{
    Task<IEnumerable<Agent>> GetAllAgentsAsync();
    Task<Agent?> GetAgentByIdAsync(int id);
    Task<Agent> CreateAgentAsync(Agent agent);
    Task<bool> UpdateAgentAsync(Agent agent);
    Task<IEnumerable<Agent>> GetAvailableAgentsAsync(int? teamId = null);
    Task<bool> IncrementTicketCountAsync(int agentId);
    Task<bool> DecrementTicketCountAsync(int agentId);
    Task<bool> DeleteAgentAsync(int id);
    Task<bool> AssignToTeamAsync(int agentId, int teamId);
}
