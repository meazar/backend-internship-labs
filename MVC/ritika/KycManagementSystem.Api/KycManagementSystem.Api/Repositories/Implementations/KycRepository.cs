using Dapper;
using KycManagementSystem.Api.Database;
using KycManagementSystem.Api.Models.DTOs.Kyc;
using KycManagementSystem.Api.Models.DTOs.Pagination;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;

namespace KycManagementSystem.Api.Repositories.Implementations
{
    public class KycRepository:IKycRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        public KycRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<int> CreateKycAsync(KycProfileDto kycProfile)
        {
            string sql = @"
        INSERT INTO KycProfiles
        (
            UserId, FullName, DateOfBirth, Gender, Address, 
            FatherName, GrandfatherName, Occupation, AnnualIncome, 
            Nationality, PhotoPath,  Remarks, CreatedAt
        )
        VALUES
        (
            @UserId, @FullName, @DateOfBirth, @Gender, @Address,
            @FatherName, @GrandfatherName, @Occupation, @AnnualIncome,
            @Nationality, @PhotoPath,
             @Remarks, GETDATE()
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT);
    ";

            using var conn = _connectionFactory.CreateConnection();

            int kycId = await conn.QuerySingleAsync<int>(sql, kycProfile);

            string logSql = @"
        INSERT INTO KycHistory (KycId, Action, Remarks, ActionDate)
        VALUES (@KycId, 'Submitted', @Remarks, GETDATE());
    ";

            await conn.ExecuteAsync(logSql, new { KycId = kycId, Remarks = kycProfile.Remarks });

            return kycId;
        }

        public async Task<KycProfileDto?> GetByIdAsync(int id)
        {
            const string sql = "Select * from KycProfiles where Id=@Id";
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<KycProfileDto>(sql, new { Id = id });

        }
        public async Task<IEnumerable<KycProfileDto>> GetPagedAsync(PageRequest pageRequest)
        {
            string sql = @"Select * from KycProfiles 
                Order by CreatedAt Desc
                Offset @offset Rows
                fetch next @PageSize Rows only";
            using var conn = _connectionFactory.CreateConnection();
            var kycProfiles = await conn.QueryAsync<KycProfileDto>(sql, new
            {
                offset = (pageRequest.PageNumber -1)* pageRequest.PageSize,
                PageSize = pageRequest.PageSize
            });
            return kycProfiles;


        }
        public async Task UpdateKycAsync(KycProfileDto kycProfile)
        {
            const string sql = @"
            Update KycProfiles
            Set FullName = @FullName,
                DateOfBirth = @DateOfBirth,
                Gender = @Gender,
                Address = @Address,
                FatherName = @FatherName,
                GrandfatherName = @GrandfatherName,
                Occupation = @Occupation,
                AnnualIncome = @AnnualIncome,
                Nationality = @Nationality,
                PhotoPath = @PhotoPath,
                
                Remarks = @Remarks,
                UpdatedAt = GETDATE()
            Where Id = @Id
        ";

            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, kycProfile);

            const string logSql = @"
            Insert into KycHistory (KycId, Action, Remarks, ActionDate)
            Values (@KycId, 'Updated', @Remarks, GETDATE());
        ";
            await conn.ExecuteAsync(logSql, new { KycId = kycProfile.Id, Remarks = kycProfile.Remarks });
        }
        public async Task UpdateStatusAsync(int kycId, string status, int? officerId, string? remarks)
        {
            const string sql = @"
            Update KycProfiles
            Set Status = @Status,
                UpdatedAt = GETDATE(),
                Remarks = @Remarks
            Where Id = @KycId
        ";

            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, new { KycId = kycId, Status = status, Remarks = remarks });

            const string logSql = @"
            Insert into KycHistory (KycId, Action, OfficerId, Remarks, ActionDate)
            Values (@KycId, @Action, @OfficerId, @Remarks, GETDATE());
        ";

            await conn.ExecuteAsync(logSql, new { KycId = kycId, Action = status, OfficerId = officerId, Remarks = remarks });

        }
        public async Task<int> CountAsync()
        {
            const string sql = "Select Count(*) from KycProfiles";
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QuerySingleAsync<int>(sql);
        }
        

    }
}
