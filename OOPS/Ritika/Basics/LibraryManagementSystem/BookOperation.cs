using System;
using System.Collections.Generic;
//using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace LibraryManagementSystem
{
    public static class BookOperations
    {
        public static void AddBook(Book book)
        {
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Books (Title, Author, Year) VALUES (@Title, @Author, @Year)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Year", book.Year);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Book added successfully.");
            }
        }

        public static void DisplayBooks()
        {
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Books";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("ID\tTitle\tAuthor\tYear");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["BookId"]}\t{reader["Title"]}\t{reader["Author"]}\t{reader["Year"]}");
                }
            }
        }

        [Obsolete]
        public static void EditBook(int BookId, Book book)
        {
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                string query = "UPDATE Books SET Title=@Title, Author=@Author, Year=@Year WHERE BookId=@BookId";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Year", book.Year);
                cmd.Parameters.AddWithValue("@BookId", BookId);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Book updated successfully." : "Book not found.");
            }
        }

        public static void DeleteBook(int BookId)
        {
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Books WHERE BookId=@BookId"; // updated column name
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@BookId", BookId);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Book deleted successfully." : "Book not found.");
            }
        }

    }
}
