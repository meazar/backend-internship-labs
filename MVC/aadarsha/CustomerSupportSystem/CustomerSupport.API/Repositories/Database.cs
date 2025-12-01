using System.Data;
using Microsoft.Data.SqlClient;

namespace CustomerSupport.API.Repositories;

public class Database
{
    private readonly IConfiguration _config;
    private readonly string _connStr;

    public Database(IConfiguration config)
    {
        _config = config;
        _connStr =
            _config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found."
            );
    }

    public IDbConnection GetConnection() => new SqlConnection(_connStr);
}
