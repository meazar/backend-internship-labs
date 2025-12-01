namespace CustomerSupport.API.DTOs;

public class NotificationDto
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string Type { get; set; } = "other";
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
