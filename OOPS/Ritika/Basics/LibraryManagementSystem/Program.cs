using System;
using LibraryManagementSystem;

namespace Basics
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.CreateDatabaseAndTable();

            while (true)
            {
                Console.WriteLine("\nLibrary Management System");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. Display Books");
                Console.WriteLine("3. Edit Book");
                Console.WriteLine("4. Delete Book");
                Console.WriteLine("5. Exit");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Book book = new Book();
                        Console.Write("Enter Title: "); book.Title = Console.ReadLine();
                        Console.Write("Enter Author: "); book.Author = Console.ReadLine();
                        Console.Write("Enter Year: "); book.Year = Convert.ToInt32(Console.ReadLine());
                        BookOperations.AddBook(book);
                        break;

                    case "2":
                        BookOperations.DisplayBooks();
                        break;

                    case "3":
                        Console.Write("Enter Book ID to edit: ");
                        int editId = Convert.ToInt32(Console.ReadLine());
                        Book editBook = new Book();
                        Console.Write("Enter new Title: "); editBook.Title = Console.ReadLine();
                        Console.Write("Enter new Author: "); editBook.Author = Console.ReadLine();
                        Console.Write("Enter new Year: "); editBook.Year = Convert.ToInt32(Console.ReadLine());
                        BookOperations.EditBook(editId, editBook);
                        break;

                    case "4":
                        Console.Write("Enter Book ID to delete: ");
                        int deleteId = Convert.ToInt32(Console.ReadLine());
                        BookOperations.DeleteBook(deleteId);
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}
