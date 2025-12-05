using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Entities;

public class Book : BaseEntity
{
    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? ISBN { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public int TotalCopies { get; set; } = 1;

    [Required]
    public int AvailableCopies { get; set; } = 1;

    [Required]
    public string? Publisher { get; set; } = string.Empty;

    [Range(1000, 2100)]
    public int? PublicationYear { get; set; }

    [Required]
    public string? Language { get; set; } = "English";

    public int? NumberOfPages { get; set; }

    public string? CoverImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public virtual ICollection<BookCategory> BookCategories { get; set; } =
        new List<BookCategory>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
