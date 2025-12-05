/*using LoanManagementSystem.Database;
using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace LoanManagementSystem.Repository
{
    public class AuditRepository : IAuditRepository
    {

        private readonly IDatabaseService _databaseService;

        public AuditRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> LogActionAsync(AuditLog auditLog)
        {
            const string sql = @"INSERT INTO AuditLogs (UserId, Action, TableName, OldValue, NewValue, Timestamp)
                OUTPUT INSERTED.AuditId
                VALUES (@UserId, @Action, @TableName, @OldValue, @NewValue, @Timestamp)";

            var auditId = await _databaseService.ExecuteScalarAsync(sql,
                new SqlParameter("@UserId", auditLog.UserId),
                new SqlParameter("@Action", auditLog.Action),
                new SqlParameter("@TableName", auditLog.TableName),
                new SqlParameter("@OldValue", auditLog.OldValue ?? (object)DBNull.Value),
                new SqlParameter("@NewValue", auditLog.NewValue ?? (object)DBNull.Value),
                new SqlParameter("@Timestamp", auditLog.Timestamp));

            return auditId != null ? Convert.ToInt32(auditId) : 0;

        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var sql = @"
                SELECT AuditId, UserId, Action, TableName, OldValue, NewValue, Timestamp
                FROM AuditLogs 
                WHERE 1=1";

            var parameters = new List<SqlParameter>();

            if (fromDate.HasValue)
            {
                sql += "AND Timestamp >= @FromDate";
                parameters.Add(new SqlParameter("@ToDate", toDate.Value));

            }


            if (toDate.HasValue)
            {
                sql += "AND Timestamp <= @ToDate";
                parameters.Add(new SqlParameter("@ToDate", toDate.Value));

            }

            sql += "ORDER BY Timestamp DESC";

            var auditLogs = new List<AuditLog>();
            using var reader = await _databaseService.ExecuteReaderAsync(sql, parameters.ToArray());

            while(await reader.ReadAsync())
            {
                auditLogs.Add(new AuditLog
                {
                    AuditId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Action = reader.GetString(2),
                    TableName = reader.GetString(3),
                    OldValue = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    NewValue = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Timestamp = reader.GetDateTime(6)
                });
            }

            return auditLogs;

        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByUserAsync(int userId)
        {
            const string sql = @"
                SELECT AuditId, UserId, Action, TableName, OldValue, NewValue, Timestamp
                FROM AuditLogs 
                WHERE UserId = @UserId 
                ORDER BY Timestamp DESC";

            var auditLogs = new List<AuditLog>();
            using var reader = await _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@UserId", userId));

            while(await reader.ReadAsync())
            {
                auditLogs.Add(new AuditLog
                {
                    AuditId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Action = reader.GetString(2),
                    TableName = reader.GetString(3),
                    OldValue = reader.IsDBNull(4) ? null : reader.GetString(4),
                    NewValue = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Timestamp = reader.GetDateTime(6)
                });
            }

            return auditLogs;
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByTableAsync(string tableName)
        {
            const string sql = @"
                SELECT AuditLogId, UserId, Action, TableName, OldValue, Timestamp FROM AuditLogs
                WHERE TableName = @TableName
                ORDER BY Timestamp DESC";

            var auditLogs = new List<AuditLog>();
            using var reader = await _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@TableName", tableName));

            while(await reader.ReadAsync())
            {
                auditLogs.Add(new AuditLog
                {
                    AuditId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Action = reader.GetString(2),
                    TableName = reader.GetString(3),
                    OldValue = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    NewValue = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Timestamp = reader.GetDateTime(6)
                });
            }
            return auditLogs;
        }
    }
}
*/