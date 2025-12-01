using Dapper;
using KycManagementSystem.Api.Database;
using KycManagementSystem.Api.Models.DTOs.Pagination;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;

namespace KycManagementSystem.Api.Repositories.Implementations
{
    public class LogsRepository: ILogsRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        public LogsRepository (ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> LogAsync(ApiLog log)
        {
            string sql = @"INSERT INTO ApiLogs(Path, Method, StatusCode, RequestBody, ResponseBody, CreatedAt)
                           OUTPUT INSERTED.Id
                           VALUES (@Path, @Method, @StatusCode, @RequestBody, @ResponseBody, @CreatedAt);";
            using var conn = _connectionFactory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, log);    

        }

        public async Task<IEnumerable<ApiLog>> GetAllLogsAsync()
        {
            string sql = "Select * from ApiLogs Order By CreatedAt Desc";
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<ApiLog>(sql);
        }
        public async Task<PageResult<ApiLog>> GetPagedLogsAsync(PageRequest request)
        {
            using var conn = _connectionFactory.CreateConnection();

            string sql = @"SELECT * FROM ApiLogs
                   ORDER BY CreatedAt DESC
                   OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                   SELECT COUNT(*) FROM ApiLogs;";

            var multi = await conn.QueryMultipleAsync(sql, new
            {
                Offset = (request.PageNumber - 1) * request.PageSize,
                PageSize = request.PageSize
            });

            var logs = await multi.ReadAsync<ApiLog>();
            var totalCount = await multi.ReadSingleAsync<int>();

            return new PageResult<ApiLog>(logs, totalCount, request.PageNumber, request.PageSize);
        }

    }
}
