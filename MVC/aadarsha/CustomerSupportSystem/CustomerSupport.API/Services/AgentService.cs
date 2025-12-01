using CustomerSupport.API.Models;
using CustomerSupport.API.Repositories;

namespace CustomerSupport.API.Services;

public class AgentService : IAgentService
{
    private readonly IAgentRepository _repo;

    public AgentService(IAgentRepository repo) => _repo = repo;

    public Task<IEnumerable<Agent>> GetAllAgentsAsync() => _repo.GetAllAgentsAsync();

    public Task<Agent?> GetAgentByIdAsync(int id) => _repo.GetAgentByIdAsync(id);

    public Task<Agent> CreateAgentAsync(Agent agent)
    {
        agent.CreatedAt = agent.CreatedAt == default ? DateTime.UtcNow : agent.CreatedAt;
        agent.CurrentTickets = agent.CurrentTickets == default ? 0 : agent.CurrentTickets;
        return _repo.CreateAgentAsync(agent);
    }

    public async Task<Agent?> UpdateAgentAsync(Agent agent)
    {
        var updated = await _repo.UpdateAgentAsync(agent);
        return updated ? agent : null;
    }

    public Task<bool> DeleteAgentAsync(int id) => _repo.DeleteAgentAsync(id);

    public Task<bool> AssignToTeamAsync(int agentId, int? teamId) =>
        _repo.AssignToTeamAsync(agentId, teamId ?? 0);
}
