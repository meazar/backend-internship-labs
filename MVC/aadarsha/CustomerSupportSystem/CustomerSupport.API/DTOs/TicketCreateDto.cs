namespace CustomerSupport.API.DTOs;

public class TicketCreateDto
{
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
