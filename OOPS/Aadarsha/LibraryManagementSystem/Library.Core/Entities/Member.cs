namespace Library.Core.Entities;

public class Member
{
    public int MemberId { get; set; }
    public string? FullName { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }

    public List<Transaction> Transactions { get; set; } = new();
}