namespace LMS.Core.DTOs.Responses;

public class ReservationResponse
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public DateTime ReservationDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? NotificationDate { get; set; }
    public string Status { get; set; } = "Pending";
    public int PositionInQueue { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
