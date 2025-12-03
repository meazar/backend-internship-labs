using KycManagementSystem.Api.Models.Entities;

namespace KycManagementSystem.Api.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<List<Notification>> GetUserNotificationsAsync(int userId);
        Task MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync(int userId);
    }
}
