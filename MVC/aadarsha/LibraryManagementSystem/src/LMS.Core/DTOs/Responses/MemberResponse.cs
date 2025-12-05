namespace LMS.Core.DTOs.Responses;

public class MemberResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? MemberId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public DateTime? MembershipStartDate { get; set; }
    public DateTime? MembershipEndDate { get; set; }
    public string MembershipType { get; set; } = "Regular";
    public int MaxBooksAllowed { get; set; }
    public int CurrentBooksCheckedOut { get; set; }
    public decimal TotalFines { get; set; }
    public bool IsMembershipActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
