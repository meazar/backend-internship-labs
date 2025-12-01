using CustomerSupport.API.Models;
using Dapper;

namespace CustomerSupport.API.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly Database _db;

    public NotificationRepository(Database db) => _db = db;

    public async Task<Notification> CreateNotificationAsync(int userId, Notification notification)
    {
        if (notification == null)
            throw new ArgumentNullException(nameof(notification));

        using var conn = _db.GetConnection();

        // Validate ticket exists
        var ticketExists = await conn.ExecuteScalarAsync<int>(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM tickets WHERE id = @Id) THEN 1 ELSE 0 END",
            new { Id = notification.TicketId }
        );
        if (ticketExists == 0)
            throw new KeyNotFoundException($"Ticket with id {notification.TicketId} not found.");

        // Validate user exists
        var userExists = await conn.ExecuteScalarAsync<int>(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM users WHERE id = @Id) THEN 1 ELSE 0 END",
            new { Id = userId }
        );
        if (userExists == 0)
            throw new KeyNotFoundException($"User with id {userId} not found.");

        var sql =
            @"
            INSERT INTO notifications (ticket_id, user_id, title, message, type, is_read)
            OUTPUT INSERTED.id, INSERTED.ticket_id, INSERTED.user_id, INSERTED.title, INSERTED.message, INSERTED.type, INSERTED.is_read, INSERTED.created_at
            VALUES (@TicketId, @UserId, @Title, @Message, @Type, @IsRead);
            SELECT LAST_INSERT_ID();
        ";

        var inserted = await conn.QuerySingleAsync<Notification>(
            sql,
            new
            {
                TicketId = notification.TicketId,
                UserId = userId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type ?? "other",
                IsRead = notification.IsRead,
            }
        );

        return inserted;
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(
        int userId,
        bool onlyUnread = false
    )
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
            SELECT
                id,
                ticket_id AS TicketId,
                user_id AS UserId,
                title,
                message,
                type,
                is_read AS IsRead,
                created_at AS CreatedAt
            FROM notifications
            WHERE user_id = @UserId
        ";

        if (onlyUnread)
        {
            sql += " AND is_read = 0";
        }
        sql += " ORDER BY created_at DESC;";
        return await conn.QueryAsync<Notification>(sql, new { UserId = userId });
    }

    public async Task<bool> MarkAsReadAsync(int notificationId)
    {
        using var conn = _db.GetConnection();

        var sql =
            @"
            UPDATE notifications
            SET is_read = 1
            WHERE id = @Id;
        ";
        var rows = await conn.ExecuteAsync(sql, new { Id = notificationId });
        return rows > 0;
    }
}
