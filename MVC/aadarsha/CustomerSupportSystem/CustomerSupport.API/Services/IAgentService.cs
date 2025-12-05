using CustomerSupport.API.Models;

namespace CustomerSupport.API.Services;

public interface IAgentService
{
    Task<IEnumerable<Agent>> GetAllAgentsAsync();
    Task<Agent?> GetAgentByIdAsync(int id);
    Task<Agent> CreateAgentAsync(Agent agent);
    Task<Agent?> UpdateAgentAsync(Agent agent);
    Task<bool> DeleteAgentAsync(int id);
    Task<bool> AssignToTeamAsync(int agentId, int? teamId);
}
