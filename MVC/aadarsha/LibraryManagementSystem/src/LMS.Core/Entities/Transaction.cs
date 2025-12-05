using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Entities;

public class Transaction : BaseEntity
{
    public int MemberId { get; set; }
    public virtual Member Member { get; set; } = null!;

    public int BookId { get; set; }
    public virtual Book Book { get; set; } = null!;

    public DateTime CheckoutDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "CheckedOut"; // CheckedOut, Returned, Overdue, Lost

    [MaxLength(500)]
    public string? Notes { get; set; }

    public decimal? LateFee { get; set; }
    public int? RenewalCount { get; set; } = 0;

    // Navigation to Fine
    public virtual Fine? Fine { get; set; }
}
