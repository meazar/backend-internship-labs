namespace Library.Core.Entities;
public class Author
{
    public int AuthorId { get; set; }
    public string? FullName { get; set; }

    public bool IsActive { get; set; } = true;
    public List<BookAuthor> BooksAuthors { get; set; } = new();
}