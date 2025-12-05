using CustomerSupport.API.Models;

namespace CustomerSupport.API.Repositories;

public interface INotificationRepository
{
    Task<Notification> CreateNotificationAsync(int userId, Notification notification);
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(
        int userId,
        bool onlyUnread = false
    );
    Task<bool> MarkAsReadAsync(int notificationId);
}
