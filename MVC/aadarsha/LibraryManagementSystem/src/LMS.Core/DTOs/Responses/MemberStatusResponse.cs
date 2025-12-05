namespace LMS.Core.DTOs.Responses;

public class MemberStatusResponse
{
    public int MemberId { get; set; }
    public bool CanBorrow { get; set; }
    public string Status { get; set; } = string.Empty;
    public int BooksCheckedOut { get; set; }
    public int BooksAllowed { get; set; }
    public decimal PendingFines { get; set; }
    public bool HasOverdueBooks { get; set; }
    public int ActiveReservations { get; set; }
}
