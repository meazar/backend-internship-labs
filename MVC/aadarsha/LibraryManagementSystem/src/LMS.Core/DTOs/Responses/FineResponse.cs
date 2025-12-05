namespace LMS.Core.DTOs.Responses;

public class FineResponse
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int? TransactionId { get; set; }
    public string? BookTitle { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = "Overdue";
    public string? Description { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string Status { get; set; } = "Pending";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
