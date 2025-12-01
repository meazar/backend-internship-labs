using CustomerSupport.API.DTOs;
using CustomerSupport.API.Models;
using CustomerSupport.API.Repositories;

namespace CustomerSupport.API.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;

    public NotificationService(INotificationRepository repo) => _repo = repo;

    public async Task<NotificationDto> CreateNotificationAsync(
        int userId,
        NotificationCreateDto dto
    )
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
        if (dto.TicketId <= 0)
            throw new ArgumentException("Ticket must be provided.");

        var entity = new Notification
        {
            TicketId = dto.TicketId,
            // UserId is passed separately, repository sets it
            Title = dto.Title,
            Message = dto.Message,
            Type = string.IsNullOrWhiteSpace(dto.Type) ? "other" : dto.Type,
            IsRead = false,
        };

        var created = await _repo.CreateNotificationAsync(userId, entity);

        return new NotificationDto
        {
            Id = created.Id,
            TicketId = created.TicketId,
            UserId = created.UserId,
            Title = created.Title,
            Message = created.Message,
            Type = created.Type,
            IsRead = created.IsRead,
            CreatedAt = created.CreatedAt,
        };
    }

    public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(
        int userId,
        bool onlyUnread = false
    )
    {
        var rows = await _repo.GetNotificationsByUserIdAsync(userId, onlyUnread);

        return rows.Select(n => new NotificationDto
        {
            Id = n.Id,
            TicketId = n.TicketId,
            UserId = n.UserId,
            Title = n.Title,
            Message = n.Message,
            Type = n.Type,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt,
        });
    }

    public async Task<bool> MarkAsReadAsync(int notificationId)
    {
        return await _repo.MarkAsReadAsync(notificationId);
    }
}
