namespace CustomerSupport.API.Models;

public class Agent
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TeamId { get; set; }
    public bool IsAvailable { get; set; }
    public int MaxTickets { get; set; }
    public int CurrentTickets { get; set; }
    public DateTime CreatedAt { get; set; }

    // navigation properties (optional)
    public string? Name { get; set; }
    public string? Email { get; set; }
}
