using CustomerSupport.API.Models;

namespace CustomerSupport.API.Services;

public interface ITeamService
{
    Task<IEnumerable<Team>> GetAllTeamsAsync();
    Task<Team?> GetTeamByIdAsync(int id);
    Task<Team> CreateTeamAsync(Team team);
    Task<Team?> UpdateTeamAsync(Team team);
    Task<bool> DeleteTeamAsync(int id);
    Task<bool> AssignAgentToTeamAsync(int teamId, int agentId);
    Task<bool> RemoveAgentsFromTeamAsync(int teamId);
}
