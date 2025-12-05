using CustomerSupport.API.Models;
using Dapper;

namespace CustomerSupport.API.Repositories;

public class AgentRepository : IAgentRepository
{
    private readonly Database _db;

    public AgentRepository(Database db) => _db = db;

    public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
            SELECT
                id AS Id,
                user_id AS UserId,
                team_id AS TeamId,
                is_available AS IsAvailable,
                max_tickets AS MaxTickets,
                current_tickets AS CurrentTickets,
                created_at AS CreatedAt
            FROM agents
            ORDER BY id;
        ";
        return await conn.QueryAsync<Agent>(sql);
    }

    public async Task<Agent?> GetAgentByIdAsync(int id)
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
            SELECT
                id AS Id,
                user_id AS UserId,
                team_id AS TeamId,
                is_available AS IsAvailable,
                max_tickets AS MaxTickets,
                current_tickets AS CurrentTickets,
                created_at AS CreatedAt
            FROM agents
            WHERE id = @Id;
        ";
        return await conn.QueryFirstOrDefaultAsync<Agent>(sql, new { Id = id });
    }

    public async Task<Agent> CreateAgentAsync(Agent agent)
    {
        if (agent == null)
            throw new ArgumentNullException(nameof(agent));

        using var conn = _db.GetConnection();

        // Validate user exists and user_type = 'agent'
        var userExits = await conn.ExecuteScalarAsync<int>(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM users WHERE id = @UserId AND user_type = 'agent') THEN 1 ELSE 0 END;",
            new { agent.UserId }
        );

        if (userExits == 0)
            throw new KeyNotFoundException(
                $"User with ID {agent.UserId} does not exist or is not of type 'agent'."
            );

        var sql =
            @"
           INSERT INTO agents (user_id, team_id, is_available, max_tickets, current_tickets, created_at)
            OUTPUT INSERTED.id, INSERTED.user_id AS UserId, INSERTED.team_id AS TeamId,
                   INSERTED.is_available AS IsAvailable, INSERTED.max_tickets AS MaxTickets,
                   INSERTED.current_tickets AS CurrentTickets, INSERTED.created_at AS CreatedAt
            VALUES (@UserId, @TeamId, @IsAvailable, @MaxTickets, @CurrentTickets, GETDATE());
        ";

        var inserted = await conn.QuerySingleAsync<Agent>(
            sql,
            new
            {
                UserId = agent.UserId,
                TeamId = agent.TeamId,
                IsAvailable = agent.IsAvailable,
                MaxTickets = agent.MaxTickets,
                CurrentTickets = agent.CurrentTickets,
            }
        );

        return inserted;
    }

    public async Task<IEnumerable<Agent>> GetAvailableAgentsAsync(int? teamId = null)
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
            SELECT
                a.id,
                a.user_id AS UserId,
                a.team_id AS TeamId,
                a.is_available AS IsAvailable,
                a.max_tickets AS MaxTickets,
                a.current_tickets AS CurrentTickets,
                a.created_at AS CreatedAt,
                u.name As Name,
                u.email AS Email
            FROM agents a
            INNER JOIN users u ON a.user_id = u.id
            WHERE a.is_available = 1
            AND u.user_type IN ('agent', 'admin')";

        if (teamId.HasValue)
        {
            sql += " AND a.team_id = @TeamId";
        }

        sql += " ORDER BY a.current_tickets ASC;";

        return await conn.QueryAsync<Agent>(sql, new { TeamId = teamId });
    }

    public async Task<bool> IncrementTicketCountAsync(int agentId)
    {
        using var conn = _db.GetConnection();

        var affectedRows = await conn.ExecuteAsync(
            @"
            UPDATE agents
            SET current_tickets = current_tickets + 1
            WHERE id = @AgentId
            AND current_tickets < max_tickets",
            new { AgentId = agentId }
        );
        return affectedRows > 0;
    }

    public async Task<bool> DecrementTicketCountAsync(int agentId)
    {
        using var conn = _db.GetConnection();

        var affectedRows = await conn.ExecuteAsync(
            @"
            UPDATE agents
            SET current_tickets = GREATEST(0, current_tickets - 1) 
            WHERE id = @AgentId",
            new { AgentId = agentId }
        );
        return affectedRows > 0;
    }

    public async Task<bool> UpdateAgentAsync(Agent agent)
    {
        if (agent == null)
            throw new ArgumentNullException(nameof(agent));

        using var conn = _db.GetConnection();

        var sql =
            @"
            UPDATE agents
            SET
                team_id = @TeamId,
                is_available = @IsAvailable,
                max_tickets = @MaxTickets
            WHERE id = @Id;
        ";

        var rows = await conn.ExecuteAsync(
            sql,
            new
            {
                Id = agent.Id,
                TeamId = agent.TeamId,
                IsAvailable = agent.IsAvailable,
                MaxTickets = agent.MaxTickets,
            }
        );
        return rows > 0;
    }

    public async Task<bool> DeleteAgentAsync(int id)
    {
        using var conn = _db.GetConnection();
        var rows = await conn.ExecuteAsync("DELETE FROM agents WHERE id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> AssignToTeamAsync(int agentId, int teamId)
    {
        using var conn = _db.GetConnection();

        // Ensure the team exists
        var exists = await conn.ExecuteScalarAsync<int>(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM teams WHERE id = @Id) THEN 1 ELSE 0 END",
            new { Id = teamId }
        );
        if (exists == 0)
            throw new KeyNotFoundException($"Team with id {teamId} not found.");

        var sql =
            @"
            UPDATE agents
            SET team_id = @TeamId
            WHERE id = @AgentId;
        ";
        var affectedRows = await conn.ExecuteAsync(sql, new { AgentId = agentId, TeamId = teamId });
        return affectedRows > 0;
    }
}
