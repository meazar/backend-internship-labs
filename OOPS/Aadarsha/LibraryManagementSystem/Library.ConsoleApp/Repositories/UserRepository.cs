using Microsoft.Data.SqlClient;
using Library.ConsoleApp.Helpers;
using Library.Core.Entities;

namespace Library.ConsoleApp.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseHelper _db;

        public UserRepository(DatabaseHelper db)
        {
            _db = db;
        }

        // Register
        public void RegisterUser(string username, string password, string role)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            string query = @"INSERT INTO Users (Username, Password, Role)
                     VALUES (@Username, @Password, @Role)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Role", role);

            cmd.ExecuteNonQuery();
        }

        // Login
        public User? Login(string username, string password)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            string query = @"SELECT UserId, Username, Role 
                             FROM Users 
                             WHERE Username = @Username AND Password = @Password";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Role = reader.GetString(reader.GetOrdinal("Role"))
                };
            }

            return null;
        }

        // Add user                // ============================================================
        public void AddUser(string username, string password, string role)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand(
                @"INSERT INTO Users (Username, Password, Role) 
                  VALUES (@Username, @Password, @Role)", conn);

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Role", role);

            cmd.ExecuteNonQuery();
        }

        // Get all users
        public List<User> GetAllUsers()
        {
            List<User> users = new();

            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand(
                @"SELECT UserId, Username, Role 
                  FROM Users ORDER BY UserId ASC", conn);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Role = reader.GetString(2)
                });
            }
            return users;
        }
    }
}
