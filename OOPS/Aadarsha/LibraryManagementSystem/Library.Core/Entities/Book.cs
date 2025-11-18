namespace Library.Core.Entities;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public bool Status { get; set; } = true;
    public List<BookAuthor> BookAuthors { get; set; } = new();
    public List<BookCategory> BookCategories { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
}