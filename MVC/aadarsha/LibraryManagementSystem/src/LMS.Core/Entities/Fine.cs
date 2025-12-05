using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Entities;

public class Fine : BaseEntity
{
    public int MemberId { get; set; }
    public virtual Member Member { get; set; } = null!;

    public int? TransactionId { get; set; }
    public virtual Transaction? Transaction { get; set; }

    public decimal Amount { get; set; }

    [MaxLength(20)]
    public string Reason { get; set; } = "Overdue"; // Overdue, Lost, Damage

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? PaymentDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Paid, Waived, Cancelled

    public bool IsActive { get; set; } = true;
}
