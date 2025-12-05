using LoanManagementSystem.Database;
using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace LoanManagementSystem.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IDatabaseService _databaseService;

        public NotificationRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> CreateNotificationAsync(Notification notification)
        {
            const string sql = @"INSERT INTO Notifications (UserId, Title, Message, Type, IsRead, CreatedAt)
                OUTPUT INSERTED.NotificationId
                VALUES (@UserId, @Title, @Message, @Type, @IsRead, @CreatedAt)";

            var notificationId = await _databaseService.ExecuteScalarAsync(sql,
                new SqlParameter("UserId",notification.UserId),
                new SqlParameter("@Title", notification.Title),
                new SqlParameter("@Message", notification.Message),
                new SqlParameter("@Type", notification.Type),
                new SqlParameter("@IsRead", notification.IsRead),
                new SqlParameter("@CreatedAt", notification.CreatedAt));

            return notificationId != null ? Convert.ToInt32( notificationId ): 0;
        }


        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            const string sql = @"SELECT NotificationId, UserId, Title, Message, Type, IsRead, CreatedAt
                FROM Notifications 
                WHERE UserId = @UserId 
                ORDER BY CreatedAt DESC";

            var notifications = new List<Notification>();
            using var reader = await _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@UserId", userId));


            while(await reader.ReadAsync())
            {
                notifications.Add(new Notification
                {
                    NotificationId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Message = reader.GetString(3),
                    Type = reader.GetString(4),
                    IsRead = reader.GetBoolean(5),
                    CreatedAt = reader.GetDateTime(6)

                });

            }
            return notifications;
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            const string sql = @"UPDATE Notifications 
                SET IsRead = 1
                WHERE NotificationId = @NotificationId";

            var rowsAffected = await _databaseService.ExecuteNonQueryAsync(sql,
                new SqlParameter("@NotificationId",notificationId));

            return rowsAffected > 0;


        }

        public async Task<bool> MarkAllNotificationsAsReadAsync(int userId)
        {
            const string sql = @"
                UPDATE Notifications 
                SET IsRead = 1
                WHERE UserId = @UserId AND IsRead = 0";

            var rowsAffected = await _databaseService.ExecuteNonQueryAsync(sql,
                new SqlParameter("@UserId", userId));

            return rowsAffected > 0;

        }

        public async Task<int> GetUnreadNotificationCountAsync(int userId)
        {

            const string sql = @"
                SELECT COUNT(*) 
                FROM Notifications 
                WHERE UserId = @UserId AND IsRead = 0";

            var result = await _databaseService.ExecuteScalarAsync(sql,
                new SqlParameter("@UserId",userId));

            return result != null? Convert.ToInt32(result) : 0;

        }
    }
}
