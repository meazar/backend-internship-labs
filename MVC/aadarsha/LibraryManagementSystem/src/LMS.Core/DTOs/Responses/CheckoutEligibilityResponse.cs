namespace LMS.Core.DTOs.Responses;

public class CheckoutEligibilityResponse
{
    public bool CanCheckout { get; set; }
    public string Message { get; set; } = string.Empty;
    public int BooksCheckedOut { get; set; }
    public int MaxBooksAllowed { get; set; }
    public decimal PendingFines { get; set; }
    public bool HasOverdueBooks { get; set; }
    public bool IsMemberActive { get; set; }
    public bool IsBookAvailable { get; set; }
}
