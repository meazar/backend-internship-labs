using Dapper;
using KycManagementSystem.Api.Database;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace KycManagementSystem.Api.Repositories.Implementations
{
    public class KycHistoryRepository : IKycHistoryRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public KycHistoryRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddHistoryAsync(KycHistory history)
        {
            using var conn = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO KycHistory (KycId, Action, Remarks, OfficerId, ActionDate)
                VALUES (@KycId, @Action, @Remarks, @OfficerId, @ActionDate);";

            await conn.ExecuteAsync(sql, history);
        }

        public async Task<IEnumerable<KycHistory>> GetHistoryByKycIdAsync(int kycId)
        {
            using var conn= _connectionFactory.CreateConnection();
            string sql = @"SELECT * FROM KycHistory WHERE KycId = @Id ORDER BY ActionDate DESC";

            return await conn.QueryAsync<KycHistory>(sql, new { Id = kycId });
        }
    }
}
