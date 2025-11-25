using Microsoft.Data.SqlClient;
using Library.ConsoleApp.Helpers;
using Library.Core.Entities;

namespace Library.ConsoleApp.Repositories
{
    public class LibraryRepository
    {
        private readonly DatabaseHelper _db;
        public LibraryRepository(DatabaseHelper db) => _db = db;

        // Add Member
        public void AddMember(string fullName, string email)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                string query = @"INSERT INTO Members (FullName, Email)
                                 VALUES (@FullName, @Email)";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@Email", email);

                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                    throw new Exception("A member with this email already exists.");

                throw;
            }
        }

        // Add Book
        public void AddBook(string title, string isbn, int totalCopies,
                            List<int> authorIds, List<int> categoryIds)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var tran = conn.BeginTransaction();

            try
            {
                // Insert book
                using var cmdBook = new SqlCommand(@"
                    INSERT INTO Books (Title, ISBN, TotalCopies, AvailableCopies, Status)
                    VALUES (@Title, @ISBN, @TotalCopies, @TotalCopies, 1);
                    SELECT SCOPE_IDENTITY();", conn, tran);

                cmdBook.Parameters.AddWithValue("@Title", title);
                cmdBook.Parameters.AddWithValue("@ISBN", isbn);
                cmdBook.Parameters.AddWithValue("@TotalCopies", totalCopies);

                int bookId = Convert.ToInt32(cmdBook.ExecuteScalar());

                // Insert authors
                foreach (int authorId in authorIds)
                {
                    using var cmdBA = new SqlCommand(
                        "INSERT INTO BookAuthors (BookId, AuthorId) VALUES (@BookId, @AuthorId)",
                        conn, tran);

                    cmdBA.Parameters.AddWithValue("@BookId", bookId);
                    cmdBA.Parameters.AddWithValue("@AuthorId", authorId);
                    cmdBA.ExecuteNonQuery();
                }

                // Insert categories
                foreach (int categoryId in categoryIds)
                {
                    using var cmdBC = new SqlCommand(
                        "INSERT INTO BookCategories (BookId, CategoryId) VALUES (@BookId, @CategoryId)",
                        conn, tran);

                    cmdBC.Parameters.AddWithValue("@BookId", bookId);
                    cmdBC.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmdBC.ExecuteNonQuery();
                }

                tran.Commit();
                Console.WriteLine("Book added successfully with authors and categories!");
            }
            catch (Exception ex)
            {
                tran.Rollback();
                Console.WriteLine($"Error adding book: {ex.Message}");
            }
        }

        // Get Transactions by Member
        public List<Transaction> GetTransactionsByMember(int memberId)
        {
            var transactions = new List<Transaction>();

            using var conn = _db.GetConnection();
            conn.Open();

            string query = @"
                SELECT t.TransactionId, t.BookId, t.MemberId, t.IssueDate, 
                       t.DueDate, t.ReturnDate, t.FineAmount,
                       b.Title, b.ISBN
                FROM Transactions t
                INNER JOIN Books b ON t.BookId = b.BookId
                WHERE t.MemberId = @MemberId
                ORDER BY t.IssueDate DESC";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MemberId", memberId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                transactions.Add(new Transaction
                {
                    TransactionId = reader.GetInt32(reader.GetOrdinal("TransactionId")),
                    BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                    MemberId = reader.GetInt32(reader.GetOrdinal("MemberId")),
                    IssueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                    DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                    ReturnDate = reader["ReturnDate"] as DateTime?,
                    FineAmount = reader["FineAmount"] as decimal?,
                    Book = new Book
                    {
                        BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                        Title = reader["Title"]?.ToString() ?? "",
                        ISBN = reader["ISBN"]?.ToString() ?? ""
                    }
                });
            }

            return transactions;
        }

        // Issue Book
        public void IssueBook(int bookId, int memberId, DateTime dueDate)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand("sp_IssueBook", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@BookId", bookId);
            cmd.Parameters.AddWithValue("@MemberId", memberId);
            cmd.Parameters.AddWithValue("@DueDate", dueDate);

            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Book issued successfully!");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error issuing book: {ex.Message}");
            }
        }

        // View Books
        public void ViewBooks()
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand(
                "SELECT BookId, Title, ISBN, TotalCopies, AvailableCopies FROM Books",
                conn);

            using var reader = cmd.ExecuteReader();

            Console.WriteLine("--- Books in Library: ---");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["BookId"]} | {reader["Title"]} | ISBN: {reader["ISBN"]} | Total: {reader["TotalCopies"]} | Available: {reader["AvailableCopies"]}");
            }
        }

        // Get Authors
        public List<(int Id, string Name)> GetAuthors()
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand(
                "SELECT AuthorId, FullName FROM Authors WHERE IsActive = 1",
                conn);

            using var reader = cmd.ExecuteReader();

            var authors = new List<(int, string)>();

            while (reader.Read())
            {
                authors.Add((reader.GetInt32(0), reader.GetString(1)));
            }

            return authors;
        }

        // Get Categories
        public List<(int Id, string Name)> GetCategories()
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand("SELECT CategoryId, Name FROM Categories", conn);

            using var reader = cmd.ExecuteReader();

            var categories = new List<(int, string)>();

            while (reader.Read())
            {
                categories.Add((reader.GetInt32(0), reader.GetString(1)));
            }

            return categories;
        }

        // Return Book
        public void ReturnBook(int transactionId)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand("sp_ReturnBook", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TransactionId", transactionId);
            cmd.ExecuteNonQuery();

            Console.WriteLine("Book returned successfully!");
        }
    }
}
