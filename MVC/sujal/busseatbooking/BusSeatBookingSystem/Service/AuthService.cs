using BusSeatBookingSystem.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSeatBookingSystem.Service
{
    public class AuthService
    {
        private readonly DatabaseServer _dbServer;
        private User _currentUser;

        public AuthService(DatabaseServer dbServer)
        {
            _dbServer = dbServer;
        }

        public User CurrentUser => _currentUser;

        public bool Login(string email, string password)
        {
            using var connection = _dbServer.GetConnection();
            connection.Open();

            var query = "SELECT UserId, Name, Email, Password, Role FROM Users WHERE Email = @Email";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var storedPassword = reader["Password"].ToString();

                if (BCrypt.Net.BCrypt.Verify(password, storedPassword))
                {
                    _currentUser = new User
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Role = reader["Role"].ToString(),

                    };
                    return true;
                }
            }
            return false;

        }

        //private bool VerifyPassword(string password, string storedPassword)
        //{
        //    return BCrypt.Net.BCrypt.Verify(password, storedPassword);
        //}

        public void Register(string name, string email, string password, string role)
        {
            using var connection = _dbServer.GetConnection();
            connection.Open();

            var query = "INSERT INTO Users (Name, Email, Password, Role) VALUES (@Name, @Email, @Password, @Role)";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", BCrypt.Net.BCrypt.HashPassword(password));
            command.Parameters.AddWithValue("@Role", role);

            command.ExecuteNonQuery();
        }


        public void LogOut()
        {
            _currentUser = null;
        }
    }

}
