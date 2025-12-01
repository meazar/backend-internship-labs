using LoanManagementSystem.Database;
using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace LoanManagementSystem.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseService _databaseService;

        public UserRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            const string sql = @"
                    SELECT UserId,FullName,Email,Password,PhoneNumber,Role,KYCStatus,DateOfBirth, Address, CreatedAt, UpdatedAt
                    FROM Users WHERE UserId =@UserId";

            using var reader = await _databaseService.ExecuteReaderAsync(sql, new SqlParameter("@UserId", userId));

            if(await reader.ReadAsync())
            {
                return new User
                {
                    UserId = reader.GetInt32(0),
                    FullName= reader.GetString(1),
                    Email= reader.GetString(2),
                    Password = reader.GetString(3),
                    PhoneNumber = reader.GetString(4),
                    Role = reader.GetString(5),
                    KYCStatus = reader.GetString(6),
                    DateOfBirth = reader.GetDateTime(7),
                    Address = reader.GetString(8),
                    CreatedAt = reader.GetDateTime(9),
                    UpdatedAt   = reader.GetDateTime(10),


                };
            }
            return null;


        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {

            const string sql = @"
                SELECT UserId, FullName, Email, Password, PhoneNumber, Role, KYCStatus, DateOfBirth, Address, CreatedAt, UpdatedAt FROM Users WHERE Email = @Email";

            var reader = await _databaseService.ExecuteReaderAsync(sql, new SqlParameter("@Email", email));

            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    Password = reader.GetString(3),
                    PhoneNumber = reader.GetString(4),
                    Role = reader.GetString(5),
                    KYCStatus = reader.GetString(6),
                    DateOfBirth = reader.GetDateTime(7),
                    Address = reader.GetString(8),
                    CreatedAt = reader.GetDateTime(9),
                    UpdatedAt = reader.GetDateTime(10),

                };

            }
            return null;


        }

        public async Task<int> CreateUserAsync(CreateUserRequest user)
        {
            const string sql = @"
                INSERT INTO Users (FullName, Email, Password, PhoneNumber, Role, KYCStatus, DateOfBirth, Address, CreatedAt, UpdatedAt)
                OUTPUT INSERTED.UserId
                VALUES(@FullName,@Email,@Password,@PhoneNumber,@Role,@KYCStatus, @DateOfBirth, @Address,GETUTCDATE(),GETUTCDATE());";


            var userId = await _databaseService.ExecuteScalarAsync(sql,
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Password", BCrypt.Net.BCrypt.HashPassword(user.Password)),
                new SqlParameter("@PhoneNumber", user.PhoneNumber),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@KYCStatus","Pending"),
                new SqlParameter("@DateOfBirth", user.DateOfBirth),
                new SqlParameter("@Address", user.Address));

            return userId != null ? Convert.ToInt32(userId) : 0;
        }

        public async Task<bool> UpdatedUserAsync(User user)
        {
            const string sql = @"
                UPDATE Users 
                SET FullName = @FullName, PhoneNumber = @PhoneNumber, DateOfBirth = @DateOfBirth, Address = @Address , UpdatedAt = GETDATE()
                WHERE UserId = @UserId";

            var rowsAffected = await _databaseService.ExecuteNonQueryAsync(sql,
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@PhoneNumber", user.PhoneNumber),
                new SqlParameter("@DateOfBirth", user.DateOfBirth),
                new SqlParameter("@Address", user.Address),
                new SqlParameter("@UserId", user.UserId));
            return rowsAffected > 0;   
        }

        public async Task<bool> UpdatedKYCStatusAsync(int userId, string status)
        {
            const string sql = @"
                UPDATE users
                SET KYCStatus = @KYCStatus, UpdatedAt = GETDATE()
                WHERE UserId = @UserId";

            var rowsAffected = await _databaseService.ExecuteNonQueryAsync(sql,
                   new SqlParameter("@KYCStatus", status),
                   new SqlParameter("@UserId", userId));

            return rowsAffected > 0;

        }

        public async Task<IEnumerable<User>> GetUserByRoleAsync(string role)
        {
            const string sql = @"
                SELECT UserId, FullName, Email, Password, PhoneNumber, Role, KYCStatus, DateOfBirth, Address, createdAt, UpdatedAt
                FROM Users WHERE Role = @Role";

            var users = new List<User>();
            using var reader = await _databaseService.ExecuteReaderAsync(sql, new SqlParameter("@Role", role));


            while(await reader.ReadAsync())
            {
                users.Add(new User
                {
                    UserId = reader.GetInt32(0),
                    FullName= reader.GetString(1),
                    Email = reader.GetString (2),
                    Password = reader.GetString(3),
                    PhoneNumber = reader.GetString(4),
                    Role = reader.GetString(5),
                    KYCStatus = reader.GetString(6),
                    DateOfBirth = reader.GetDateTime(7),
                    Address = reader.GetString(8),
                    CreatedAt = reader.GetDateTime(9),
                    UpdatedAt = reader.GetDateTime(10),


                });
            }
            return users;
        }
    }
}
