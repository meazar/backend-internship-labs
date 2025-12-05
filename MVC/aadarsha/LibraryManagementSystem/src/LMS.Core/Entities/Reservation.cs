using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Entities;

public class Reservation : BaseEntity
{
    public int MemberId { get; set; }
    public virtual Member Member { get; set; } = null!;

    public int BookId { get; set; }
    public virtual Book Book { get; set; } = null!;

    public DateTime ReservationDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? NotificationDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Available, Cancelled, Expired

    public int PositionInQueue { get; set; } = 1;

    [MaxLength(500)]
    public string? Notes { get; set; }
}
