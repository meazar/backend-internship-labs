namespace LMS.Core.DTOs.Responses;

public class BookResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ISBN { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
    public int? PublicationYear { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
