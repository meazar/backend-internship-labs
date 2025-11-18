namespace Library.Core.Entities;

public class Transaction
{
    public int TransactionId { get; set; }

    public int BookId { get; set; }
    public int MemberId { get; set; }

    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public decimal? FineAmount { get; set; }

    // navigation
    public Book? Book { get; set; }
    public Member? Member { get; set; }
}