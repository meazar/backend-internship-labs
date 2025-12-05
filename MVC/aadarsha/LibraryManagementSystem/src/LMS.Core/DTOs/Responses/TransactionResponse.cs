namespace LMS.Core.DTOs.Responses;

public class TransactionResponse
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public DateTime CheckoutDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = "CheckedOut";
    public decimal? LateFee { get; set; }
    public int RenewalCount { get; set; }
    public bool IsOverdue { get; set; }
    public int OverdueDays { get; set; }
    public DateTime CreatedAt { get; set; }
}
