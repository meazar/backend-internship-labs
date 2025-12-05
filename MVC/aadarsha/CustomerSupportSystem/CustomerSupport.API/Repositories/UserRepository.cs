using CustomerSupport.API.Models;
using Dapper;

namespace CustomerSupport.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Database _db;

    public UserRepository(Database db) => _db = db;

    public async Task<User?> GetUserById(int id)
    {
        using var conn = _db.GetConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT id, email, password_hash as PasswordHash, name, user_type as UserType, created_at, updated_at FROM users WHERE Id = @Id",
            new { Id = id }
        );
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        using var conn = _db.GetConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT id, email, password_hash as PasswordHash, name, user_type as UserType, created_at, updated_at FROM users WHERE email = @Email ",
            new { Email = email }
        );
    }

    public async Task<User> CreateUser(User user)
    {
        using var conn = _db.GetConnection();
        var sql =
            @"
            INSERT INTO users (email, password_hash, name, user_type)
            VALUES (@Email, @PasswordHash, @Name, @UserType);
            SELECT CAST(SCOPE_IDENTITY() as int);";
        var newId = await conn.QuerySingleAsync<int>(sql, user);
        user.Id = newId;
        return user;
    }

    public async Task<User?> DeleteUser(int id)
    {
        using var conn = _db.GetConnection();
        var user = await GetUserById(id);
        if (user == null)
            return null;

        return await conn.QueryFirstOrDefaultAsync<User>(
            "DELETE FROM users WHERE Id = @Id; SELECT * FROM users WHERE Id = @Id;",
            new { Id = id }
        );
    }

    public async Task<IEnumerable<User?>> GetAllUsers()
    {
        using var conn = _db.GetConnection();
        return await conn.QueryAsync<User>("SELECT id, name, email FROM users");
    }
}
