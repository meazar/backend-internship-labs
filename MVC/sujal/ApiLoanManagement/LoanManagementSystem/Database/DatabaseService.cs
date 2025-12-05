using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;

namespace LoanManagementSystem.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;
        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            }

        }
        public async Task<SqlConnection> GetConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;


        }

        public async Task<int> ExecuteNonQueryAsync(string sql, params SqlParameter[] parameters)
        {
            using var connection = await GetConnectionAsync();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<object> ExecuteScalarAsync(string sql, params SqlParameter[] parameters)
        {
            using var connection = await GetConnectionAsync();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);
            return await command.ExecuteScalarAsync();
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string sql, params SqlParameter[] parameters)
        {
            var connection = await GetConnectionAsync();
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);
            return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }


    }
}
