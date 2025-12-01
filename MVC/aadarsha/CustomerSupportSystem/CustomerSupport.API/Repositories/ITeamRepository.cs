using CustomerSupport.API.Models;

namespace CustomerSupport.API.Repositories;

public interface ITeamRepository
{
    Task<IEnumerable<Team>> GetAllTeamsAsync();
    Task<Team?> GetTeamByIdAsync(int id);
    Task<Team> CreateTeamAsync(Team team);
    Task<bool> UpdateTeamAsync(Team team);
    Task<bool> DeleteTeamAsync(int id);
    Task<bool> IsTeamInBusyAsync(int id);
    Task<bool> AssignAgentsToTeamAsync(int teamId, IEnumerable<int> agentIds);
    Task<bool> RemoveAgentsFromTeamAsync(int teamId);
}
