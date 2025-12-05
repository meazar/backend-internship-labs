using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Entities;

public class Member : BaseEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;

    [MaxLength(20)]
    public string? MemberId { get; set; } // Library card number

    public DateTime? MembershipStartDate { get; set; }
    public DateTime? MembershipEndDate { get; set; }

    [MaxLength(20)]
    public string? MembershipType { get; set; } = "Regular"; // Regular, Premium, Student, etc.

    public int MaxBooksAllowed { get; set; } = 5;
    public int CurrentBooksCheckedOut { get; set; } = 0;

    public decimal TotalFines { get; set; } = 0;
    public bool IsMembershipActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();
}
