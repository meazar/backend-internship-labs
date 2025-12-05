namespace CustomerSupport.API.DTOs;

public class NotificationCreateDto
{
    public int TicketId { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }

    // Type of notification(ticket_created, ticket_assigned, new_message, ticket_resolved, ticket_closed, please_confirm_resolution, other)
    public string Type { get; set; } = "other";
}
