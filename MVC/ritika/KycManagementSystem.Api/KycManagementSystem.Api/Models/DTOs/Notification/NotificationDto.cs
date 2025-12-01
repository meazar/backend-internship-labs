namespace KycManagementSystem.Api.Models.DTOs.Notification
{
    public class NotificationDto
    {
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? Type { get; set; }
    }
}
