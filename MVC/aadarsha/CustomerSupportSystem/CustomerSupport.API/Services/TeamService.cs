using CustomerSupport.API.Models;
using CustomerSupport.API.Repositories;

namespace CustomerSupport.API.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _repo;

    public TeamService(ITeamRepository repo) => _repo = repo;

    public Task<IEnumerable<Team>> GetAllTeamsAsync() => _repo.GetAllTeamsAsync();

    public Task<Team?> GetTeamByIdAsync(int id) => _repo.GetTeamByIdAsync(id);

    public Task<Team> CreateTeamAsync(Team team)
    {
        team.CreatedAt = team.CreatedAt == default ? DateTime.UtcNow : team.CreatedAt;
        return _repo.CreateTeamAsync(team);
    }

    public async Task<Team?> UpdateTeamAsync(Team team)
    {
        var updated = await _repo.UpdateTeamAsync(team);
        return updated ? team : null;
    }

    public Task<bool> DeleteTeamAsync(int id) => _repo.DeleteTeamAsync(id);

    public async Task<bool> AssignAgentToTeamAsync(int teamId, int agentId)
    {
        var team = await _repo.GetTeamByIdAsync(teamId);
        if (team == null)
            return false;

        try
        {
            var added = false;

            // If Team has an Agents collection, try to add an Agent instance with Id = agentId
            var agentsProperty = team.GetType().GetProperty("Agents");
            if (agentsProperty != null)
            {
                var agents = agentsProperty.GetValue(team) as System.Collections.IList;
                if (agents != null)
                {
                    var elementType = agents.GetType().IsGenericType
                        ? agents.GetType().GetGenericArguments()[0]
                        : null;
                    if (elementType != null)
                    {
                        var agentInstance = Activator.CreateInstance(elementType);
                        var idProp = elementType.GetProperty("Id");
                        if (idProp != null && idProp.CanWrite)
                        {
                            idProp.SetValue(agentInstance, agentId);
                            agents.Add(agentInstance);
                            added = true;
                        }
                    }
                }
            }

            // If Team has an AgentIds collection (e.g., List<int>), try to add the agentId
            if (!added)
            {
                var agentIdsProperty = team.GetType().GetProperty("AgentIds");
                if (agentIdsProperty != null)
                {
                    var agentIds = agentIdsProperty.GetValue(team) as System.Collections.IList;
                    if (agentIds != null)
                    {
                        agentIds.Add(agentId);
                        added = true;
                    }
                }
            }

            if (!added)
                return false;

            var updated = await _repo.UpdateTeamAsync(team);
            return updated;
        }
        catch
        {
            return false;
        }
    }

    public Task<bool> RemoveAgentsFromTeamAsync(int teamId) =>
        _repo.RemoveAgentsFromTeamAsync(teamId);
}
