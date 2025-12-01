namespace CustomerSupport.API.DTOs;

public class AgentUpdateDto
{
    public int Id { get; set; }

    public int? TeamId { get; set; }
    public bool IsAvailable { get; set; }
    public int MaxTickets { get; set; }
}
