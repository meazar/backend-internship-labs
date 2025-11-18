using Microsoft.Data.SqlClient;
using Library.ConsoleApp.Helpers;

namespace Library.ConsoleApp.Repositories
{
    public class AuthorRepository
    {
        private readonly DatabaseHelper _db;
        public AuthorRepository(DatabaseHelper db) => _db = db;

        public void AddAuthor(string fullName, string bio = null!)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("dbo.sp_AddAuthor", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@FullName", fullName);
            cmd.Parameters.AddWithValue("@Bio", (object)bio ?? DBNull.Value);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Author added.");
        }

        public List<(int Id, string FullName, bool IsActive)> GetAllAuthors()
        {
            var list = new List<(int, string, bool)>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("dbo.sp_GetAllAuthors", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(reader.GetOrdinal("AuthorId"));
                string name = reader.GetString(reader.GetOrdinal("FullName"));
                bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                list.Add((id, name, isActive));
            }
            return list;
        }
    }
}
