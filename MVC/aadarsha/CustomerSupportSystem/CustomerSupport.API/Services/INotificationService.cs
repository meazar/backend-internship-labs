using CustomerSupport.API.DTOs;

namespace CustomerSupport.API.Services;

public interface INotificationService
{
    Task<NotificationDto> CreateNotificationAsync(int userId, NotificationCreateDto dto);
    Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(
        int userId,
        bool onlyUnread = false
    );
    Task<bool> MarkAsReadAsync(int notificationId);
}
