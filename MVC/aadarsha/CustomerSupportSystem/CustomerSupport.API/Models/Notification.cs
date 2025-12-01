namespace CustomerSupport.API.Models;

public class Notification
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string Type { get; set; } = "other"; // 'ticket_created', 'ticket_assigned', 'new_message', 'ticket_resolved', 'ticket_closed', 'please_confirm_resolution', 'other'
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
