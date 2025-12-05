namespace LMS.Core.Entities;

public class BookAuthor : BaseEntity
{
    public int BookId { get; set; }
    public virtual Book Book { get; set; } = null!;

    public int AuthorId { get; set; }
    public virtual Author Author { get; set; } = null!;

    // Additional fields for junction table
    public string? Role { get; set; } // "Author", "Editor", "Translator", "Illustrator"
    public int Order { get; set; } = 1; // Order of authors (first author, second author, etc.)
}
