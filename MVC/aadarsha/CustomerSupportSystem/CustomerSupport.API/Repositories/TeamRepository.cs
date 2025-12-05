using CustomerSupport.API.Models;
using Dapper;

namespace CustomerSupport.API.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly Database _db;

    public TeamRepository(Database db) => _db = db;

    public async Task<IEnumerable<Team>> GetAllTeamsAsync()
    {
        using var conn = _db.GetConnection();
        var sql =
            @"
            SELECT id AS Id, name AS Name, email AS Email, is_active AS IsActive, created_at AS CreatedAt
            FROM teams
            ORDER BY id;
        ";
        return await conn.QueryAsync<Team>(sql);
    }

    public async Task<Team?> GetTeamByIdAsync(int id)
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
            SELECT id AS Id, name AS Name, email AS Email, is_active AS IsActive, created_at AS CreatedAt
            FROM teams
            WHERE id = @Id;
        ";

        return await conn.QueryFirstOrDefaultAsync<Team>(sql, new { Id = id });
    }

    public async Task<Team> CreateTeamAsync(Team team)
    {
        using var conn = _db.GetConnection();
        var sql =
            @"
            INSERT INTO teams (name, email, is_active, created_at)
            OUTPUT INSERTED.id, INSERTED.name AS Name, INSERTED.email AS Email, INSERTED.is_active AS IsActive, INSERTED.created_at AS CreatedAt
            VALUES (@Name, @Email, @IsActive, GETDATE());
        ";

        var created = await conn.QuerySingleAsync<Team>(
            sql,
            new
            {
                Name = team.Name,
                Email = team.Email,
                IsActive = team.IsActive,
            }
        );
        return created;
    }

    public async Task<bool> UpdateTeamAsync(Team team)
    {
        using var conn = _db.GetConnection();
        var sql =
            @"
            UPDATE teams
            SET name = @Name,
                email = @Email,
                is_active = @IsActive
            WHERE id = @Id;
        ";
        var affectedRows = await conn.ExecuteAsync(
            sql,
            new
            {
                team.Name,
                team.Email,
                team.IsActive,
                team.Id,
            }
        );
        return affectedRows > 0;
    }

    public async Task<bool> DeleteTeamAsync(int id)
    {
        using var conn = _db.GetConnection();

        var rows = await conn.ExecuteAsync("DELETE FROM teams WHERE id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> IsTeamInBusyAsync(int id)
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
            SELECT COUNT(1)
            FROM agents
            WHERE team_id = @Id;
        ";

        var count = await conn.ExecuteScalarAsync<int>(sql, new { Id = id });
        return count > 0;
    }

    public async Task<bool> AssignAgentToTeamAsync(int teamId, int agentId)
    {
        using var conn = _db.GetConnection();

        // Validate team exists
        var teamExists = await conn.ExecuteScalarAsync<int>(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM teams WHERE id = @Id) THEN 1 ELSE 0 END",
            new { Id = teamId }
        );
        if (teamExists == 0)
            throw new KeyNotFoundException($"Team {teamId} not found.");

        // Validate agent exists
        var agentExists = await conn.ExecuteScalarAsync<int>(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM agents WHERE id = @Id) THEN 1 ELSE 0 END",
            new { Id = agentId }
        );
        if (agentExists == 0)
            throw new KeyNotFoundException($"Agent {agentId} not found.");

        // Assign
        var sql = "UPDATE agents SET team_id = @TeamId WHERE id = @AgentId;";
        var rows = await conn.ExecuteAsync(sql, new { TeamId = teamId, AgentId = agentId });
        return rows > 0;
    }

    public async Task<bool> AssignAgentsToTeamAsync(int teamId, IEnumerable<int> agentIds)
    {
        using var conn = _db.GetConnection();
        using var tran = conn.BeginTransaction();
        try
        {
            // Validate team exists
            var teamExists = await conn.ExecuteScalarAsync<int>(
                "SELECT CASE WHEN EXISTS (SELECT 1 FROM teams WHERE id = @Id) THEN 1 ELSE 0 END",
                new { Id = teamId },
                tran
            );
            if (teamExists == 0)
                throw new KeyNotFoundException($"Team {teamId} not found.");

            if (agentIds == null)
                return false;

            // Ensure agentIds is not empty
            var hasAny = false;
            foreach (var _ in agentIds)
            {
                hasAny = true;
                break;
            }
            if (!hasAny)
                return false;

            // Validate all agents exist
            var agentCount = await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM agents WHERE id IN @AgentIds",
                new { AgentIds = agentIds },
                tran
            );

            var providedCount = 0;
            foreach (var _ in agentIds)
                providedCount++;

            if (agentCount != providedCount)
                throw new KeyNotFoundException("One or more agents not found.");

            // Assign all agents to the team
            var sql = "UPDATE agents SET team_id = @TeamId WHERE id IN @AgentIds;";
            var rows = await conn.ExecuteAsync(
                sql,
                new { TeamId = teamId, AgentIds = agentIds },
                tran
            );

            tran.Commit();
            return rows > 0;
        }
        catch
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<bool> RemoveAgentsFromTeamAsync(int teamId)
    {
        using var conn = _db.GetConnection();

        // Ensure team has agents
        var belongs = await conn.ExecuteScalarAsync<int>(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM agents WHERE team_id = @TeamId) THEN 1 ELSE 0 END",
            new { TeamId = teamId }
        );
        if (belongs == 0)
            return false;

        var sql = "UPDATE agents SET team_id = NULL WHERE team_id = @TeamId;";
        var rows = await conn.ExecuteAsync(sql, new { TeamId = teamId });
        return rows > 0;
    }
}
