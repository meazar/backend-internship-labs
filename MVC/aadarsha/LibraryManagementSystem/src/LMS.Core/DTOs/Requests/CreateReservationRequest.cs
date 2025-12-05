using System.ComponentModel.DataAnnotations;

namespace LMS.Core.DTOs.Requests;

public class CreateReservationRequest
{
    [Required]
    public int MemberId { get; set; }

    [Required]
    public int BookId { get; set; }

    [Range(1, 30)]
    public int ExpiryDays { get; set; } = 7;
}
