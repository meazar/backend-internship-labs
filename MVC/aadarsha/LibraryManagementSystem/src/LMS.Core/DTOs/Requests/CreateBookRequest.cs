using System.ComponentModel.DataAnnotations;

namespace LMS.Core.DTOs.Requests;

public class CreateBookRequest
{
    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? ISBN { get; set; }

    [MaxLength(100)]
    public string? Author { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(1, 1000)]
    public int TotalCopies { get; set; } = 1;

    [Range(1000, 2100)]
    public int? PublicationYear { get; set; }
}
