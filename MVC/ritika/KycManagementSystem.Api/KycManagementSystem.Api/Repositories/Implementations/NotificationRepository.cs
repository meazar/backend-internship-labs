using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;

namespace KycManagementSystem.Api.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IConfiguration _config;

        public NotificationRepository(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        public async Task AddAsync(Notification notification)
        {
            var sql = @"INSERT INTO Notifications (UserId, Title, Message, Type, IsRead, CreatedAt)
                        VALUES (@UserId, @Title, @Message, @Type, 0, GETDATE())";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, notification);
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(int userId)
        {
            var sql = @"SELECT * FROM Notifications 
                        WHERE UserId = @UserId 
                        ORDER BY CreatedAt DESC";

            using var conn = CreateConnection();
            var results = await conn.QueryAsync<Notification>(sql, new { UserId = userId });
            return results.ToList();
        }

        public async Task MarkAsReadAsync(int id)
        {
            var sql = @"UPDATE Notifications SET IsRead = 1 WHERE Id = @Id";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var sql = @"UPDATE Notifications SET IsRead = 1 WHERE UserId = @UserId";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { UserId = userId });
        }
    }
}
