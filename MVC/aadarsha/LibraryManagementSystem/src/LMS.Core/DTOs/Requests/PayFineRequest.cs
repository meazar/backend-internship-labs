using System.ComponentModel.DataAnnotations;

namespace LMS.Core.DTOs.Requests;

public class PayFineRequest
{
    [Required]
    [Range(0.01, 1000)]
    public decimal Amount { get; set; }

    [MaxLength(100)]
    public string PaymentMethod { get; set; } = "Cash";

    [MaxLength(500)]
    public string? Notes { get; set; }
}
