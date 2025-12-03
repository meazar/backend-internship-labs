using Dapper;
using KycManagementSystem.Api.Database;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;

namespace KycManagementSystem.Api.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        public UserRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int>CreateUserAsync(User user)
        {
            string sql = @"
                Insert into Users(Username, Email,Password,Role,IsActive)
                values (@Username, @Email, @Password, @Role, @IsActive);
                select cast(Scope_Identity() as int);";

            using var conn = _connectionFactory.CreateConnection();
            var id = await conn.QuerySingleAsync<int>(sql, user);
            return id;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            string sql = "Select * from Users where Id = @Id";
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });

        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            string sql = "Select * from Users Where Username =@Username";
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            string sql = "Select * from Users";
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<User>(sql);
        }
        public async Task UpdateUserAsync(User user)
        {
            string sql = @"Update Users 
                           Set Email =@Email,
                                Password=@Password,
                                Role = @Role
                                IsActive = @IsActive
                           Where Id =@Id";
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, user);
        }

        public async Task DeleteUserAsync(int id)
        {
            string sql = "Delete from Users Where Id=@Id";
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, new {Id = id});
        }

    }
}
