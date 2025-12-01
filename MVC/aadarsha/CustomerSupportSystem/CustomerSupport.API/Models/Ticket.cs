namespace CustomerSupport.API.Models;

public class Ticket
{
    public int Id { get; set; }
    public string? TicketNumber { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CustomerId { get; set; }
    public int? AssignedAgentId { get; set; }
    public int? TeamId { get; set; }
    public string Status { get; set; } = "open"; // open, in_progress, resolved, closed
    public DateTime? RespondedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
