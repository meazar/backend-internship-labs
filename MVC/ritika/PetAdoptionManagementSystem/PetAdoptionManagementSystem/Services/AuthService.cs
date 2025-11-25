using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PetAdoptionManagementSystem.Data;
using PetAdoptionManagementSystem.Models;
using PetAdoptionManagementSystem.utils;

namespace PetAdoptionManagementSystem.Services
{
    public class AuthService
    {
        private readonly DatabaseHelper _db;

        public AuthService(DatabaseHelper db)
        {
            _db = db;
        }

        public async Task Register(User user)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                string query = @"Insert into Users(FullName, Email, Password, Role, Address, Phone)
                                Values (@FullName, @Email, @Password, @Role, @Address, @Phone)";
                SqlCommand cmd = new SqlCommand(query, conn);

                string hashedPassword = PasswordHasher.Hash(user.Password);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@Role", user.Role.ToString());
                cmd.Parameters.AddWithValue("@Address", user.Address);
                cmd.Parameters.AddWithValue("@Phone", user.Phone);
                int row =await cmd.ExecuteNonQueryAsync();
                Console.WriteLine(row > 0 ? "User registered successfully." : "User Registered failed.");
            }
        }

        public User Login(string email, string password)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                string query = "Select * from Users where Email=@Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            string storedHash = reader["Password"].ToString();
                            if (PasswordHasher.Verify(password, storedHash))
                            {
                                return new User
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    FullName = reader["FullName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Password = storedHash,
                                    Role = Enum.Parse<UserRole>(reader["Role"].ToString()),
                                    Address = reader["Address"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    DateRegistered = Convert.ToDateTime(reader["DateRegistered"])
                                };
                            }

                        }
                        
                    }
                }
            }
            Console.WriteLine("Invalid email or password.");
            return null;
        }
        
    }
    
}
