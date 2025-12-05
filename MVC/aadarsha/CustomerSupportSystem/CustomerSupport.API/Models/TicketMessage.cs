namespace CustomerSupport.API.Models;

public class TicketMessage
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int AuthorId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
