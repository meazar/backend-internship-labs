using KycManagementSystem.Api.Models.DTOs.Notification;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;
using KycManagementSystem.Api.Services.Interfaces;

namespace KycManagementSystem.Api.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;

        public NotificationService(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task SendNotificationAsync(NotificationDto dto)
        {
            var entity = new Notification
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Message = dto.Message,
                Type = dto.Type
            };

            await _repo.AddAsync(entity);
        }

        public async Task<List<NotificationResponseDto>> GetNotificationsAsync(int userId)
        {
            var list = await _repo.GetUserNotificationsAsync(userId);

            return list.Select(n => new NotificationResponseDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            }).ToList();
        }

        public async Task MarkAsReadAsync(int id)
        {
            await _repo.MarkAsReadAsync(id);
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            await _repo.MarkAllAsReadAsync(userId);
        }
    }
}
