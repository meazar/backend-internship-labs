using System.Data;
using Microsoft.Data.SqlClient;


namespace KycManagementSystem.Api.Database
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
