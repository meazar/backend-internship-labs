namespace CustomerSupport.API.DTOs;

public class AgentCreateDto
{
    public int UserId { get; set; } // existing user id (must exist and have user_type = 'agent')
    public int? TeamId { get; set; } // optional
    public bool IsAvailable { get; set; } = true;
    public int MaxTickets { get; set; } = 5;
}
