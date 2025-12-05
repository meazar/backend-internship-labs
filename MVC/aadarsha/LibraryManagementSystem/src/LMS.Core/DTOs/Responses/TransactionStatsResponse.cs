namespace LMS.Core.DTOs.Responses;

public class TransactionStatsResponse
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalCheckouts { get; set; }
    public int TotalReturns { get; set; }
    public int TotalRenewals { get; set; }
    public int OverdueCount { get; set; }
    public decimal TotalFinesCollected { get; set; }
    public Dictionary<string, int> CheckoutsByDay { get; set; } = new();
    public List<string> MostBorrowedBooks { get; set; } = new();
    public List<string> MostActiveMembers { get; set; } = new();
}
