using System.ComponentModel.DataAnnotations;

namespace LMS.Core.DTOs.Requests;

public class CreateFineRequest
{
    [Required]
    public int MemberId { get; set; }

    public int? TransactionId { get; set; }

    [Required]
    [Range(0.01, 1000)]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(20)]
    public string Reason { get; set; } = "Overdue";

    [MaxLength(500)]
    public string? Description { get; set; }
}
