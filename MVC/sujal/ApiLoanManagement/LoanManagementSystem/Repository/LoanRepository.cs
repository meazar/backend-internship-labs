using LoanManagementSystem.Database;
using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace LoanManagementSystem.Repository
{
    public class LoanRepository : ILoanRepository
    {

        private readonly IDatabaseService _databaseService;
        private readonly ILogger<LoanRepository> _logger;

        public LoanRepository(IDatabaseService databaseService, ILogger<LoanRepository> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        public async Task<IEnumerable<LoanType>> GetAllLoanTypesAsync()
        {
            const string sql = @"
                SELECT LoanTypeId, LoanName, InterestRate, ProcessingFee, MaxAmount, MinAmount, MaxDurationMonths FROM LoanTypes";
            var loanTypes = new List<LoanType>();

            try
            {
                using var reader = await _databaseService.ExecuteReaderAsync(sql);

                while (await reader.ReadAsync())
                {
                    loanTypes.Add(new LoanType
                    {
                        LoanTypeId = reader.GetInt32(0),
                        LoanName = reader.GetString(1),
                        InterestRate = reader.GetDecimal(2),
                        ProcessingFee = reader.GetDecimal(3),
                        MaxAmount = reader.GetDecimal(4),
                        MinAmount = reader.GetDecimal(5),
                        MaxDurationMonths = reader.GetInt32(6),


                    });
                }

                return loanTypes;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving loan types from database");
                throw;
            }           

        }


        public async Task<LoanType?> GetLoanTypeByIdAsync(int loanTypeId)
        {
            const string sql = @"
                SELECT LoanTypeId, LoanName, InterestRate, ProcessingFee, MaxAmount,MinAmount, MaxDurationMonths 
                FROM LoanTypes 
                WHERE LoanTypeId=@LoanTypeId";


            using var reader = await _databaseService.ExecuteReaderAsync(sql, new SqlParameter("@LoanTypeId", loanTypeId));

            if (await reader.ReadAsync())
            {
                return new LoanType
                {
                    LoanTypeId = reader.GetInt32(0),
                    LoanName = reader.GetString(1),
                    InterestRate = reader.GetDecimal(2),
                    ProcessingFee = reader.GetDecimal(3),
                    MaxAmount = reader.GetDecimal(4),
                    MinAmount = reader.GetDecimal(5),
                    MaxDurationMonths = reader.GetInt32(6),
                };
            }
            return null;



        }
       

        public async Task<int> CreateLoanApplicationAsync(CreateLoanApplication  application)
        {
            const string sql = @"
                INSERT INTO LoanApplications (UserId,LoanTypeId, RequestedAmount,DurationMonths,CreatedAt)
                OUTPUT INSERTED.ApplicationId
                VALUES (@UserId, @LoanTypeId,@RequestedAmount,@DurationMonths,GETDATE())";

            var applicationId = await _databaseService.ExecuteScalarAsync(sql,
                new SqlParameter("UserId", application.UserId),
                new SqlParameter("@LoanTypeId",application.LoanTypeId),
                new SqlParameter("@RequestedAmount", application.RequestedAmount),
                new SqlParameter("@DurationMonths",application.DurationMonths));

            return applicationId != null ? Convert.ToInt32(applicationId) : 0;


        }

        public async Task <LoanApplication?> GetLoanApplicationByIdAsync(int applicationId)
        {
            const string sql = @"
                SELECT ApplicationId, UserId, LoanTypeId, RequestedAmount, DurationMonths,
                status, VerifiedBy, VerifiedAt, OfficerRemarks,createdAt
                FROM LoanApplications WHERE ApplicationId = @ApplicationId";

            
            using var reader = await _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@ApplicationId", applicationId));

            if(await reader.ReadAsync())
            {
                return new LoanApplication
                {
                    ApplicationId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    LoanTypeId = reader.GetInt32(2),
                    RequestedAmount = reader.GetDecimal(3),
                    DurationMonth = reader.GetInt32(4),
                    Status = reader.GetString(5),
                    VerifiedBy = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                    VerifiedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7),                    
                    OfficerRemarks = reader.IsDBNull(8) ? null : reader.GetString(8),
                    CreatedAt = reader.GetDateTime(9)                       

                };

            }
            return null;
        }

        public async Task<IEnumerable<LoanApplication>> GetLoanApplicationByUserIdAsync(int userId)
        {
            const string sql = @"
                SELECT ApplicationId,UserId,LoanTypeId,RequestedAmount,DurationMonths,Status, VerifiedBy, VerifiedAt, OfficerRemarks, CreatedAt FROM LoanApplications WHERE UserId = @UserId";

            var application = new List<LoanApplication>();
            using var reader = await _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@UserId", userId));

            while(await reader.ReadAsync())
            {
                application.Add(new LoanApplication
                {
                    ApplicationId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    LoanTypeId = reader.GetInt32(2),
                    RequestedAmount= reader.GetDecimal(3),
                    DurationMonth = reader.GetInt32(4),
                    Status = reader.GetString(5),
                    VerifiedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                    VerifiedBy = reader.IsDBNull (7) ? null : reader.GetInt32(7),
                    OfficerRemarks = reader.IsDBNull(8) ? null : reader.GetString(8),
                    CreatedAt = reader.GetDateTime(9),

                });
            }
            return application;

        }

        public async Task<IEnumerable<LoanApplication>> GetPendingApplicationAsync()
        {
            const string sql = @"
                SELECT ApplicationId,UserId,LoanTypeId,RequestedAmount,DurationMonths,Status, VerifiedBy,VerifiedAt, OfficerRemarks,CreatedAt FROM LoanApplications WHERE Status = 'Pending'";
            var application = new List<LoanApplication>();
            using var reader = await _databaseService.ExecuteReaderAsync(sql);

            while(await reader.ReadAsync())
            {
                application.Add (new LoanApplication
                {
                    ApplicationId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    LoanTypeId = reader.GetInt32(2),
                    RequestedAmount = reader.GetDecimal(3),
                    DurationMonth = reader.GetInt32(4),
                    Status = reader.GetString(5),
                    VerifiedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                    VerifiedBy = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                    OfficerRemarks = reader.IsDBNull(8) ? null : reader.GetString(8),
                    CreatedAt = reader.GetDateTime(9),
                });

            }
            return application;             
        }

        public async Task<bool> UpdateLoanApplicationStatusAsync(int applicationId, string status,int verifiedBy,string remarks)
        {
            const string sql = @"
                UPDATE LoanApplications
                SET Status = @Status, VerifiedBy = @VerifiedBy, VerifiedAt = GETDATE(),OfficerRemarks = @Remarks
                WHERE ApplicationId = @ApplicationId";


            var rowsaffected = await _databaseService.ExecuteNonQueryAsync(sql,
                new SqlParameter("@Status",status),
                new SqlParameter("@VerifiedBy",verifiedBy),
                new SqlParameter("@Remarks", remarks ?? (object)DBNull.Value),
                new SqlParameter("@ApplicationId",applicationId));

            return rowsaffected > 0;

        }


        public async Task<int> CreateLoanAccountAsync(LoanAccount loanAccount)
        {
            const string sql = @"
                INSERT INTO LoanAccounts (ApplicationId, UserId, PrincipalAmount,InterestRate,
                                        EMIAmount,TotalInterest,TotalPayable,BalanceAmount,
                                        PenaltyAmount, loanStatus, StartDate, EndDate)
                OUTPUT INSERTED.LoanAccountId
                VALUES (@ApplicationId,@UserId,@PrincipalAmount,@InterestRate,@EMIAmount,@TotalInterest,@TotalPayable,@BalanceAmount,@PenaltyAmount,@LoanStatus,@StartDate,@EndDate)";

            var loanAccountId = await _databaseService.ExecuteScalarAsync(sql,
                new SqlParameter("@ApplicationId", loanAccount.ApplicationId),
                new SqlParameter("@UserId", loanAccount.UserId),
                new SqlParameter("@principalAmount", loanAccount.PrincipalAmount),
                new SqlParameter("@InterestRate", loanAccount.InterestRate),
                new SqlParameter("@EMIAmount", loanAccount.EMIAmount),
                new SqlParameter("@TotalInterest", loanAccount.TotalInterest),
                new SqlParameter("@TotalPayable", loanAccount.TotalPayable),
                new SqlParameter("@BalanceAmount", loanAccount.BalanceAmount),
                new SqlParameter("@PenaltyAmount", loanAccount.PenaltyAmount),
                new SqlParameter("@LoanStatus", loanAccount.LoanStatus),
                new SqlParameter("@StartDate", loanAccount.StartDate),
                new SqlParameter("@EndDate", loanAccount.EndDate));

            return loanAccount != null ? Convert.ToInt32(loanAccountId) : 0;
        }



        public async Task<LoanAccount?> GetLoanAccountByIdAsync(int loanAccountId)
        {
            const string sql = @"
                SELECT LoanAccountId, ApplicationId, UserId, PrincipalAmount, InterestRate, 
                       EMIAmount, TotalInterest, TotalPayable, BalanceAmount, PenaltyAmount, 
                       LoanStatus, StartDate, EndDate
                FROM LoanAccounts WHERE LoanAccountId = @LoanAccountId";
            using var reader = await _databaseService.ExecuteReaderAsync(sql, new SqlParameter("@LoanAccountId", loanAccountId));

            if(await reader.ReadAsync())
            {
                return new LoanAccount
                {
                    LoanAccountId = reader.GetInt32(0),
                    ApplicationId = reader.GetInt32(1),
                    UserId = reader.GetInt32(2),
                    PrincipalAmount = reader.GetDecimal(3),
                    InterestRate = reader.GetDecimal(4),
                    EMIAmount = reader.GetDecimal(5),
                    TotalInterest = reader.GetDecimal(6),
                    TotalPayable = reader.GetDecimal(7),
                    BalanceAmount = reader.GetDecimal(8),
                    PenaltyAmount = reader.GetDecimal(9),
                    LoanStatus = reader.GetString(10),
                    StartDate = reader.GetDateTime(11),
                    EndDate = reader.GetDateTime(12)
                };
            }
            return null;

        }


        public async Task<IEnumerable<LoanAccount?>> GetLoanAccountByUserIdAsync(int userId)
        {
            const string sql = @"
                SELECT LoanAccountId, ApplicationId, UserId, PrincipalAmount, InterestRate, 
                       EMIAmount, TotalInterest, TotalPayable, BalanceAmount, PenaltyAmount, 
                       LoanStatus, StartDate, EndDate
                FROM LoanAccounts WHERE UserId = @UserId";

            var loanAccount = new List<LoanAccount>();

            using var reader = await _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@UserId", userId));

            while (await reader.ReadAsync())
            {
                loanAccount.Add(new LoanAccount
                {
                    LoanAccountId = reader.GetInt32(0),
                    ApplicationId = reader.GetInt32(1),
                    UserId = reader.GetInt32(2),
                    PrincipalAmount = reader.GetDecimal(3),
                    InterestRate = reader.GetDecimal(4),
                    EMIAmount = reader.GetDecimal(5),
                    TotalInterest = reader.GetDecimal(6),
                    TotalPayable = reader.GetDecimal(7),
                    BalanceAmount = reader.GetDecimal(8),
                    PenaltyAmount = reader.GetDecimal(9),
                    LoanStatus = reader.GetString(10),
                    StartDate = reader.GetDateTime(11),
                    EndDate = reader.GetDateTime(12)
                });
            }

            return loanAccount;

        }

        public async Task<bool> UpdateLoanAccountBalanceAsync(int loanAccountId, decimal newBalance)
        {
            const string sql = @"
                UPDATE LoanAccounts 
                SET BalanceAmount = @BalanceAmount,
                    LoanStatus = CASE WHEN @BalanceAmount = 0 THEN 'Closed' ELSE LoanStatus END
                WHERE LoanAccountId = @LoanAccountId";

            var rowAffected = await _databaseService.ExecuteNonQueryAsync(sql,
                new SqlParameter("@BalanceAmount", newBalance),
                new SqlParameter("@LoanAccountId", loanAccountId));

            return rowAffected > 0;
        }
    }

}
