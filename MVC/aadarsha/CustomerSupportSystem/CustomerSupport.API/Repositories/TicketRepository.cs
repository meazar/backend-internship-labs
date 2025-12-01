using CustomerSupport.API.Models;
using Dapper;

namespace CustomerSupport.API.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly Database _db;

    public TicketRepository(Database db) => _db = db;

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
            SELECT 
                id,
                ticket_number AS TicketNumber,
                customer_id AS CustomerId,
                subject,
                description,
                status,
                assigned_agent_id AS AssignedAgentId,
                team_id AS TeamId,
                created_at AS CreatedAt,
                updated_at AS UpdatedAt
            FROM tickets
            WHERE id = @Id;
        ";

        return await conn.QueryFirstOrDefaultAsync<Ticket>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Ticket ticket)
    {
        using var conn = _db.GetConnection();

        ticket.Status = "open";

        var sql =
            @"
            INSERT INTO tickets 
                (subject, description, customer_id, assigned_agent_id, team_id, status)
            VALUES 
                (@Subject, @Description, @CustomerId, @AssignedAgentId, @TeamId, @Status);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
        ";

        return await conn.ExecuteScalarAsync<int>(sql, ticket);
    }

    // Get all tickets of customer
    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync(int? customerId)
    {
        using var conn = _db.GetConnection();

        if (customerId.HasValue)
        {
            return await conn.QueryAsync<Ticket>(
                @"
                SELECT 
                    id,
                    ticket_number AS TicketNumber,
                    customer_id AS CustomerId,
                    subject,
                    description,
                    status,
                    assigned_agent_id AS AssignedAgentId,
                    team_id AS TeamId,
                    created_at AS CreatedAt,
                    updated_at AS UpdatedAt
                FROM tickets
                WHERE customer_id = @CustomerId
                ORDER BY created_at DESC;",
                new { CustomerId = customerId.Value }
            );
        }

        return await conn.QueryAsync<Ticket>(
            @"
            SELECT 
                id,
                ticket_number AS TicketNumber,
                customer_id AS CustomerId,
                subject,
                description,
                status,
                assigned_agent_id AS AssignedAgentId,
                created_at AS CreatedAt,
                updated_at AS UpdatedAt
            FROM tickets
            ORDER BY created_at DESC;"
        );
    }

    public async Task<TicketMessage> AddMessageAsync(int ticketId, string content, int userId)
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
            INSERT INTO ticket_messages (ticket_id, author_id, content, created_at)
            VALUES (@TicketId, @UserId, @Content, NOW())
            RETURNING 
                id,
                ticket_id AS TicketId,
                author_id AS AuthorId,
                content,
                created_at AS CreatedAt;
        ";

        return await conn.QuerySingleAsync<TicketMessage>(
            sql,
            new
            {
                TicketId = ticketId,
                UserId = userId,
                Content = content,
            }
        );
    }

    public async Task<int> AddMessageAsync(TicketMessage msg)
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
            INSERT INTO ticket_messages (ticket_id, author_id, content, created_at)
            VALUES(@TicketId, @AuthorId, @Content, NOW())
            RETURNING id;
        ";

        return await conn.ExecuteScalarAsync<int>(sql, msg);
    }

    public async Task AssignAgentAsync(int ticketId, int agentId)
    {
        using var conn = _db.GetConnection();

        await conn.ExecuteAsync(
            @"
            UPDATE tickets
            SET 
                assigned_agent_id = @AgentId,
                updated_at = GETDATE()
            WHERE id = @TicketId;
        ",
            new { AgentId = agentId, TicketId = ticketId }
        );

        await conn.ExecuteAsync(
            @"
            UPDATE agents 
            SET current_tickets = current_tickets + 1 
            WHERE id = @AgentId;
        ",
            new { AgentId = agentId }
        );
    }

    public async Task<bool> UpdateTicketStatusAsync(int ticketId, string status)
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
            UPDATE tickets
            SET
                status = @Status,
                updated_at = GETDATE()
            WHERE id = @TicketId
            ";
        var rows = await conn.ExecuteAsync(sql, new { TicketId = ticketId, Status = status });
        return rows > 0;
    }

    public async Task<bool> TeamExistsAsync(int teamId)
    {
        using var conn = _db.GetConnection();
        return await conn.ExecuteScalarAsync<bool>(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM teams WHERE id = @TeamId AND is_active = 1) THEN 1 ELSE 0 END;",
            new { TeamId = teamId }
        );
    }

    public async Task<bool> CanReassignTeamAsync(int ticketId)
    {
        using var conn = _db.GetConnection();
        var status = await conn.QueryFirstOrDefaultAsync<string>(
            "SELECT status FROM tickets WHERE id = @TicketId;",
            new { TicketId = ticketId }
        );
        return status == "open" || status == "in_progress";
    }

    public async Task<bool> AssignTeamAsync(int ticketId, int? teamId)
    {
        using var conn = _db.GetConnection();

        var affectedRows = await conn.ExecuteAsync(
            @"UPDATE tickets 
            SET team_id = @TeamId,
             updated_at = GETDATE() 
            WHERE id = @TicketId",
            new { TicketId = ticketId, TeamId = teamId }
        );

        if (affectedRows > 0 && teamId.HasValue)
        {
            await conn.ExecuteAsync(
                @"INSERT INTO notifications (ticket_id, user_id, message, type)
                    SELECT @TicketId, customer_id, 'Your ticket has been assigned to a new team.', 'team_assignment'
                FROM tickets
                WHERE id = @TicketId;",
                new { TeamId = teamId.Value }
            );
        }
        return affectedRows > 0;
    }
}
