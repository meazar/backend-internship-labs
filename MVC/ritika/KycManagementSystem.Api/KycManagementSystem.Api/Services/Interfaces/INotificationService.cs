using KycManagementSystem.Api.Models.DTOs.Notification;

namespace KycManagementSystem.Api.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(NotificationDto dto);
        Task<List<NotificationResponseDto>> GetNotificationsAsync(int userId);
        Task MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync(int userId);
    }
}
