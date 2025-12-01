using Microsoft.Data.SqlClient;

namespace LoanManagementSystem.Database
{
    public interface IDatabaseService
    {
        Task<SqlConnection> GetConnectionAsync();
        Task<int> ExecuteNonQueryAsync(string sql, params SqlParameter[] parameters);
        Task<object> ExecuteScalarAsync(string sql, params SqlParameter[] parameters);
        Task<SqlDataReader> ExecuteReaderAsync(string sql, params SqlParameter[] parameters);

    }
}
