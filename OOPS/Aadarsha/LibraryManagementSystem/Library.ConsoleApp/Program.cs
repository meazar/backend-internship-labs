using Library.ConsoleApp.Helpers;
using Library.ConsoleApp.Repositories;
using Library.ConsoleApp.Menus;

class Program
{
    static DatabaseHelper db = null!;
    static UserRepository userRepo = null!;
    static AuthorRepository authorRepo = null!;
    static LibraryRepository libraryRepo = null!;

    static void Main()
    {
        string connectionString = @"Server=localhost;Database=LibraryDB;User Id=sa;Password=aadarshaKhadka1;TrustServerCertificate=true;";
        db = new DatabaseHelper(connectionString);

        userRepo = new UserRepository(db);
        authorRepo = new AuthorRepository(db);
        libraryRepo = new LibraryRepository(db);

        while (true)
        {
            Console.Clear();
            ConsoleHelper.WriteDivider();
            ConsoleHelper.WriteCentered("LIBRARY MANAGEMENT SYSTEM - LOGIN", ConsoleColor.Cyan);
            ConsoleHelper.WriteDivider();
            ConsoleHelper.WriteCentered("Enter 'q' to Quit!", ConsoleColor.DarkYellow);

            ConsoleHelper.WriteCentered("Username: ", ConsoleColor.Yellow, false);
            string username = Console.ReadLine() ?? string.Empty;

            if (username.Equals("q", StringComparison.OrdinalIgnoreCase))
                return;

            ConsoleHelper.WriteCentered("Password: ", ConsoleColor.Yellow, false);
            string password = ReadPassword();

            var user = userRepo.Login(username, password);

            if (user == null)
            {
                ConsoleHelper.WriteCentered("Invalid username or password. Press any key to try again.", ConsoleColor.Red);
                Console.ReadKey();
                continue;
            }

            if (string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                AdminMenu();
            }
            else if (string.Equals(user.Role, "Librarian", StringComparison.OrdinalIgnoreCase))
            {
                var libMenu = new LibrarianMenu(libraryRepo, authorRepo, userRepo);
                libMenu.Show();
            }
            else
            {
                ConsoleHelper.WriteCentered("Unknown role. Contact system admin.", ConsoleColor.Red);
                Console.ReadKey();
            }
        }
    }

    static void AdminMenu()
    {
        while (true)
        {
            Console.Clear();
            ConsoleHelper.WriteDivider();
            ConsoleHelper.WriteCentered("ADMIN MENU", ConsoleColor.Green);
            ConsoleHelper.WriteDivider();
            ConsoleHelper.WriteCentered("1. Register Librarian");
            ConsoleHelper.WriteCentered("2. View All Users");
            ConsoleHelper.WriteCentered("3. Add Author");
            ConsoleHelper.WriteCentered("4. View Authors");
            ConsoleHelper.WriteCentered("5. Logout");
            ConsoleHelper.WriteDivider();
            ConsoleHelper.WriteCentered("Enter choice: ", ConsoleColor.Yellow, false);
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RegisterLibrarian();
                    break;
                case "2":
                    ViewAllUsers();
                    break;
                case "3":
                    AddAuthor();
                    break;
                case "4":
                    ViewAuthors();
                    break;
                case "5":
                    return;
                default:
                    ConsoleHelper.WriteCentered("Invalid choice. Press any key...", ConsoleColor.Red);
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void RegisterLibrarian()
    {
        try
        {
            ConsoleHelper.WriteCentered("--- Register Librarian ---", ConsoleColor.Green);

            ConsoleHelper.WriteCentered("Enter username for librarian: ", ConsoleColor.Yellow, false);
            string uname = Console.ReadLine() ?? string.Empty;
            ConsoleHelper.WriteCentered("Enter password: ", ConsoleColor.Yellow, false);
            string pwd = ReadPassword();

            userRepo.RegisterUser(uname, pwd, "Librarian");
            ConsoleHelper.WriteCentered("Librarian registered successfully! Press any key...", ConsoleColor.Green);
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteCentered($"Error registering librarian: {ex.Message}", ConsoleColor.Red);
            ConsoleHelper.WriteCentered("Press any key to continue...", ConsoleColor.Yellow);
            Console.ReadKey();
            return;
        }

    }

    static void ViewAllUsers()
    {
        var users = userRepo.GetAllUsers();
        Console.Clear();
        ConsoleHelper.WriteDivider();
        ConsoleHelper.WriteCentered("--- All Users ---", ConsoleColor.Green);
        ConsoleHelper.WriteDivider();

        Console.WriteLine("{0,-4} | {1,-15} | {2,-10}", "ID", "Username", "Role");
        ConsoleHelper.WriteDivider();
        foreach (var u in users)
        {
            Console.WriteLine("{0,-4} | {1,-15} | {2,-10}", u.Id, u.Username, u.Role);
        }
        ConsoleHelper.WriteDivider();
        ConsoleHelper.WriteCentered("Press any key to continue...", ConsoleColor.Yellow);
        Console.ReadKey();
    }

    static void AddAuthor()
    {
        ConsoleHelper.WriteCentered("Enter author full name: ", ConsoleColor.Yellow, false);
        string name = Console.ReadLine() ?? string.Empty;
        ConsoleHelper.WriteCentered("Enter short bio (optional): ", ConsoleColor.Yellow, false);
        string bio = Console.ReadLine() ?? string.Empty;

        authorRepo.AddAuthor(name, bio);
        ConsoleHelper.WriteCentered("Author added! Press any key...", ConsoleColor.Green);
        Console.ReadKey();
    }

    static void ViewAuthors()
    {
        var authors = authorRepo.GetAllAuthors();
        Console.Clear();
        ConsoleHelper.WriteDivider();
        ConsoleHelper.WriteCentered("--- Authors ---", ConsoleColor.Green);
        ConsoleHelper.WriteDivider();
        foreach (var a in authors)
        {
            ConsoleHelper.WriteCentered($"{a.Id} | {a.FullName} | Active: {a.IsActive}");
        }
        ConsoleHelper.WriteCentered("Press any key to continue...", ConsoleColor.Yellow);
        Console.ReadKey();
    }

    // --- Password Masking Method ---
    static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo info = Console.ReadKey(true);
        while (info.Key != ConsoleKey.Enter)
        {
            if (info.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password[0..^1]; // Remove last char
                    int cursorPos = Console.CursorLeft;
                    Console.SetCursorPosition(cursorPos - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(cursorPos - 1, Console.CursorTop);
                }
            }
            else
            {
                password += info.KeyChar;
                Console.Write("*"); // * for masking
            }
            info = Console.ReadKey(true);
        }
        Console.WriteLine();
        return password;
    }
}

// --- ConsoleHelper ---
static class ConsoleHelper
{
    public static void WriteCentered(string text, ConsoleColor color = ConsoleColor.White, bool newline = true)
    {
        int windowWidth = Console.WindowWidth;
        int leftPadding = Math.Max((windowWidth - text.Length) / 2, 0);

        Console.ForegroundColor = color;
        Console.SetCursorPosition(leftPadding, Console.CursorTop);
        if (newline)
            Console.WriteLine(text);
        else
            Console.Write(text);
        Console.ResetColor();
    }

    public static void WriteDivider(char ch = '═')
    {
        Console.WriteLine(new string(ch, Console.WindowWidth));
    }
}
