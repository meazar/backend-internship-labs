using Library.ConsoleApp.Repositories;

namespace Library.ConsoleApp.Menus;

public class LibrarianMenu
{
    private readonly LibraryRepository _libraryRepo;
    private readonly AuthorRepository _authorRepo;
    private readonly UserRepository _userRepo;

    public LibrarianMenu(LibraryRepository libraryRepo, AuthorRepository authorRepo, UserRepository userRepo)
    {
        _libraryRepo = libraryRepo;
        _authorRepo = authorRepo;
        _userRepo = userRepo;
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            ConsoleHelper.WriteDivider();
            ConsoleHelper.WriteCentered("LIBRARIAN MENU", ConsoleColor.Green);
            ConsoleHelper.WriteDivider();
            ConsoleHelper.WriteCentered("1. Add Member");
            ConsoleHelper.WriteCentered("2. Add Book (with Authors & Categories)");
            ConsoleHelper.WriteCentered("3. Issue Book");
            ConsoleHelper.WriteCentered("4. Return Book");
            ConsoleHelper.WriteCentered("5. View Books");
            ConsoleHelper.WriteCentered("6. Logout");
            ConsoleHelper.WriteDivider();
            ConsoleHelper.WriteCentered("Enter choice: ", ConsoleColor.Yellow, false);

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddMemberFlow();
                    break;
                case "2":
                    AddBookFlow();
                    break;
                case "3":
                    IssueBookFlow();
                    break;
                case "4":
                    ReturnBookFlow();
                    break;
                case "5":
                    _libraryRepo.ViewBooks();
                    ConsoleHelper.WriteCentered("Press any key...", ConsoleColor.Yellow);
                    Console.ReadKey();
                    break;
                case "6":
                    return;
                default:
                    ConsoleHelper.WriteCentered("Invalid choice. Press any key.", ConsoleColor.Red);
                    Console.ReadKey();
                    break;
            }
        }
    }

    public void AddMemberFlow()
    {
        ConsoleHelper.WriteCentered("Enter Full Name: ", ConsoleColor.Yellow, false);
        string name = Console.ReadLine() ?? string.Empty;

        ConsoleHelper.WriteCentered("Enter Email: ", ConsoleColor.Yellow, false);
        string email = Console.ReadLine() ?? string.Empty;

        try
        {
            _libraryRepo.AddMember(name, email);
            ConsoleHelper.WriteCentered("Member added successfully!", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteCentered($"Error: {ex.Message}", ConsoleColor.Red);
        }

        ConsoleHelper.WriteCentered("Press any key to continue...", ConsoleColor.Yellow);
        Console.ReadKey();
    }



    private void AddBookFlow()
    {
        ConsoleHelper.WriteCentered("Enter Book Title: ", ConsoleColor.Yellow, false);
        string title = Console.ReadLine() ?? string.Empty;

        ConsoleHelper.WriteCentered("Enter ISBN: ", ConsoleColor.Yellow, false);
        string isbn = Console.ReadLine() ?? string.Empty;

        ConsoleHelper.WriteCentered("Enter Total Copies: ", ConsoleColor.Yellow, false);
        int totalCopies = int.Parse(Console.ReadLine() ?? "1");

        var authors = _authorRepo.GetAllAuthors();
        ConsoleHelper.WriteDivider();
        ConsoleHelper.WriteCentered("--- Authors ---", ConsoleColor.Green);
        foreach (var a in authors)
            ConsoleHelper.WriteCentered($"{a.Id} | {a.FullName}");
        ConsoleHelper.WriteCentered("Enter Author IDs: ", ConsoleColor.Yellow, false);
        List<int> authorIds = Console.ReadLine()?.Split(',').Select(s => int.Parse(s.Trim())).ToList() ?? new List<int>();

        ConsoleHelper.WriteCentered("Enter Category IDs (existing in DB): ", ConsoleColor.Yellow, false);
        List<int> categoryIds = Console.ReadLine()?.Split(',').Select(s => int.Parse(s.Trim())).ToList() ?? new List<int>();

        _libraryRepo.AddBook(title, isbn, totalCopies, authorIds, categoryIds);
        ConsoleHelper.WriteCentered("Press any key...", ConsoleColor.Yellow);
        Console.ReadKey();
    }

    private void IssueBookFlow()
    {
        ConsoleHelper.WriteCentered("Enter Book ID to Issue: ", ConsoleColor.Yellow, false);
        int bookId = int.TryParse(Console.ReadLine(), out var bId) ? bId : 0;
        ConsoleHelper.WriteCentered("Enter Member ID: ", ConsoleColor.Yellow, false);
        int memberId = int.TryParse(Console.ReadLine(), out var mId) ? mId : 0;
        ConsoleHelper.WriteCentered("Enter Due Date (yyyy-mm-dd): ", ConsoleColor.Yellow, false);
        DateTime dueDate = DateTime.TryParse(Console.ReadLine(), out var dDate) ? dDate : DateTime.Now.AddDays(7);

        _libraryRepo.IssueBook(bookId, memberId, dueDate);
        ConsoleHelper.WriteCentered("Press any key...", ConsoleColor.Yellow);
        Console.ReadKey();
    }

    private void ReturnBookFlow()
    {
        ConsoleHelper.WriteCentered("Enter Transaction ID to Return: ", ConsoleColor.Yellow, false);
        int transactionId = int.TryParse(Console.ReadLine(), out var tId) ? tId : 0;
        _libraryRepo.ReturnBook(transactionId);
        ConsoleHelper.WriteCentered("Press any key...", ConsoleColor.Yellow);
        Console.ReadKey();
    }
}
