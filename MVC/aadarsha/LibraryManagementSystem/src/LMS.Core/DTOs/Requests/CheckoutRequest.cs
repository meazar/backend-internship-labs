using System.ComponentModel.DataAnnotations;

namespace LMS.Core.DTOs.Requests;

public class CheckoutRequest
{
    [Required]
    public int MemberId { get; set; }

    [Required]
    public int BookId { get; set; }

    [Range(1, 30)]
    public int LoanPeriodDays { get; set; } = 14;
}
