using Dapper;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Database;
using KycManagementSystem.Api.Repositories.Interfaces;

namespace KycManagementSystem.Api.Repositories.Implementations
{
    public class OfacRepository: IOfacRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        public OfacRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<bool> CheckAgainstOfacAsync(string fullName, string? nationality = null)
        {
            var sql = @"
        SELECT COUNT(*)
        FROM DummyOfacSanctionList
        WHERE FullName LIKE @FullName
    ";

            var parameters = new DynamicParameters();
            parameters.Add("@FullName", $"%{fullName}%");

            if (!string.IsNullOrEmpty(nationality))
            {
                sql += " AND Nationality = @Nationality";
                parameters.Add("@Nationality", nationality);
            }

            using var conn = _connectionFactory.CreateConnection();
            var count = await conn.QuerySingleAsync<int>(sql, parameters);

            return count > 0;
        }

        public async Task<IEnumerable<DummyOfacRecord>> GetAllSanctionsAsync()
        {
            var sql = "SELECT * FROM DummyOfacSanctionList";
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<DummyOfacRecord>(sql);
        }

        public async Task<int> AddDummySanctionAsync(DummyOfacRecord sanction)
        {
            string sql = @" Insert into DummyOfacSanctionList (FullName, AliasName, Nationality, Category, RiskLevel, Notes)
                            values (@FullName, @AliasName,@Nationality, @Category, @RiskLevel, @Notes);
                            Select Cast(Scope_Identity() as int);";
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QuerySingleAsync<int>(sql, sanction);
        }
        public async Task<IEnumerable<DummyOfacRecord>> SearchSanctionsAsync(string name, string? nationality)
        {
            var sql = @"
        SELECT *
        FROM DummyOfacSanctionList
        WHERE FullName LIKE @Name
            OR AliasName LIKE @Name
    ";

            if (!string.IsNullOrEmpty(nationality))
                sql += " AND Nationality = @Nationality";

            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<DummyOfacRecord>(sql, new
            {
                Name = $"%{name}%",
                Nationality = nationality
            });
        }



    }
}
