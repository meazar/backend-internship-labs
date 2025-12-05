using CustomerSupport.API.Models;
using Dapper;

namespace CustomerSupport.API.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly Database _db;

    public MessageRepository(Database db) => _db = db;

    public async Task<List<TicketMessage>> GetMessageByTicket(int ticketId)
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
        SELECT 
            id,
            ticket_id  AS TicketId,
            author_id  AS AuthorId,
            content,
            created_at AS CreatedAt
        FROM ticket_messages
        WHERE ticket_id = @TicketId
        ORDER BY created_at ASC;
    ";

        var result = await conn.QueryAsync<TicketMessage>(sql, new { TicketId = ticketId });

        return result.ToList();
    }

    public async Task<int> AddMessageAsync(int ticketId, string content, int authorId)
    {
        using var conn = _db.GetConnection();
        var sql =
            @"INSERT INTO ticket_messages (ticket_id, content, author_id, created_at)
                VALUES (@TicketId, @Content, @AuthorId, SYSDATETIME())";
        return await conn.ExecuteAsync(
            sql,
            new
            {
                TicketId = ticketId,
                Content = content,
                AuthorId = authorId,
            }
        );
    }
}
