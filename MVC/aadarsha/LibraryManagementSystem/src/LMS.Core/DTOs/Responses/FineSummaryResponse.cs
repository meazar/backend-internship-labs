namespace LMS.Core.DTOs.Responses;

public class FineSummaryResponse
{
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public decimal TotalPending { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalWaived { get; set; }
    public int OverdueFinesCount { get; set; }
    public int LostBookFinesCount { get; set; }
    public List<FineDetail> PendingFines { get; set; } = new();

    public class FineDetail
    {
        public int FineId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
    }
}
