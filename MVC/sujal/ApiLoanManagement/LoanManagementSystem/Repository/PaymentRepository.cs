using LoanManagementSystem.Database;
using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Data.SqlClient;

namespace LoanManagementSystem.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDatabaseService _databaseService;
        public PaymentRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> CreatePaymentScheduleAsync(PaymentSchedule schedule)
        {
            const string sql = @"
                INSERT INTO PaymentSchedule (LoanAccountId, InstallmentNo, DueDate, Amount, 
                                           PrincipalPortion, InterestPortion, Status, PenaltyAdded)
                OUTPUT INSERTED.ScheduleId
                VALUES (@LoanAccountId, @InstallmentNo, @DueDate, @Amount, 
                       @PrincipalPortion, @InterestPortion, @Status, @PenaltyAdded)";

            var scheduleId = await _databaseService.ExecuteScalarAsync(sql,
                new SqlParameter("@LoanAccountId", schedule.LoanAccountId),
                new SqlParameter("@InstallmentNo", schedule.InstallationNO),
                new SqlParameter("@DueDate", schedule.DueDate),
                new SqlParameter("@Amount", schedule.Amount),
                new SqlParameter("@PrincipalPortion", schedule.PrincipalPortion),
                new SqlParameter("@InterestPortion", schedule.InterestPortion),
                new SqlParameter("@Status", schedule.Status),
                new SqlParameter("@PenaltyAdded", schedule.PenaltyAdded));
            return scheduleId != null ? Convert.ToInt32(scheduleId) : 0;
        }


        public async Task<IEnumerable<PaymentSchedule>> GetPaymentScheduleByLoanAccountAsync(int loanAccountId)
        {
            const string sql = @"
                SELECT ScheduleId, LoanAccountId, InstallmentNo, DueDate, Amount, 
                       PrincipalPortion, InterestPortion, Status, PaidDate, PenaltyAdded
                FROM PaymentSchedule 
                WHERE LoanAccountId = @LoanAccountId 
                ORDER BY InstallmentNo";

            var schedule = new List<PaymentSchedule>();

            using var reader = await _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@LoanAccountId", loanAccountId));


            while(await reader.ReadAsync())
            {
                schedule.Add(new PaymentSchedule
                {
                    ScheduleId = reader.GetInt32(0),
                    LoanAccountId = reader.GetInt32(1),
                    InstallationNO = reader.GetInt32(2),
                    DueDate = reader.GetDateTime(3),
                    Amount = reader.GetDecimal(4),
                    PrincipalPortion = reader.GetDecimal(5),
                    InterestPortion = reader.GetDecimal(6),
                    Status = reader.GetString(7),
                    PaidDate = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                    PenaltyAdded = reader.GetDecimal(9)
                });

            }
            return schedule;


            
        }


        public async Task<PaymentSchedule?> GetPaymentScheduleByIdAsync(int scheduleId)
        {
            const string sql = @"SELECT ScheduleId, LoanAccountId, InstallmentNo, DueDate, Amount, 
                       PrincipalPortion, InterestPortion, Status, PaidDate, PenaltyAdded
                FROM PaymentSchedule WHERE ScheduleId = @ScheduleId";

            using var reader = await  _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@ScheduleId", scheduleId));

            if(await reader.ReadAsync())
            {
                return new PaymentSchedule
                {
                    ScheduleId = reader.GetInt32(0),
                    LoanAccountId = reader.GetInt32(1),
                    InstallationNO = reader.GetInt32(2),
                    DueDate = reader.GetDateTime(3),
                    Amount = reader.GetDecimal(4),
                    PrincipalPortion = reader.GetDecimal(5),
                    InterestPortion = reader.GetDecimal(6),
                    Status = reader.GetString(7),
                    PaidDate = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                    PenaltyAdded = reader.GetDecimal(9)
                };

            }
            return null;
        }

        public async Task<bool> UpdatePaymentScheduleStatusAsync(int scheduleId, string status, DateTime paidDate)
        {
            const string sql = @"UPDATE PaymentSchedule 
                SET Status = @Status, PaidDate = @PaidDate
                WHERE ScheduleId = @ScheduleId";

            var rowsAffected = await _databaseService.ExecuteNonQueryAsync(sql,
                new SqlParameter("@Status", status),
                new SqlParameter("@PaidDate", paidDate),
                new SqlParameter("@ScheduleId", scheduleId));

            return rowsAffected > 0;
        }

        public async Task<int> MakePaymentAsync(Payments payment)
        {
            const string sql = @"INSERT INTO Payments (LoanAccountId, ScheduleId, AmountPaid, PaymentMethod, 
                                    TransactionId, Remarks, PaymentDate)
                OUTPUT INSERTED.PaymentId
                VALUES (@LoanAccountId, @ScheduleId, @AmountPaid, @PaymentMethod, 
                       @TransactionId, @Remarks, @PaymentDate)";

            var paymentId = await _databaseService.ExecuteScalarAsync(sql,
                new SqlParameter("@LoanAccountId", payment.LoanAccountId),
                new SqlParameter("@ScheduleId", payment.ScheduleId),
                new SqlParameter("@AmountPaid", payment.AmountPaid),
                new SqlParameter("@PaymentMethod", payment.PaymentMethod),
                new SqlParameter("@TransactionId", payment.TransactionId),
                new SqlParameter("@Remarks", payment.Remarks ?? (object)DBNull.Value),
                new SqlParameter("@PaymentDate", payment.PaymentDate));


            return paymentId != null ? Convert.ToInt32(paymentId) : 0;

        }

        public async Task<IEnumerable<Payments>> GetPaymentsByLoanAccountAsync(int loanAccountId)
        {
            const string sql = @"
                SELECT PaymentId, LoanAccountId, ScheduleId, AmountPaid, PaymentMethod, 
                       TransactionId, PaymentDate, Remarks
                FROM Payments 
                WHERE LoanAccountId = @LoanAccountId 
                ORDER BY PaymentDate DESC";

            var payments = new List<Payments>();
            using var reader = await _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@LoanAccountId", loanAccountId));

            while(await reader.ReadAsync())
            {
                payments.Add(new Payments
                {
                    PaymentId = reader.GetInt32(0),
                    LoanAccountId = reader.GetInt32(1),
                    ScheduleId = reader.GetInt32(2),
                    AmountPaid = reader.GetDecimal(3),
                    PaymentMethod = reader.GetString(4),
                    TransactionId = reader.GetString(5),
                    PaymentDate = reader.GetDateTime(6),
                    Remarks = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
                });
            }
            return payments;


        }

        public async Task<bool> AddPenaltyAsync(Penalty penalty)
        {
            const string sql = @"INSERT INTO Penalties (LoanAccountId, ScheduleId, PenaltyAmount, Status, CreatedAt)
                VALUES (@LoanAccountId, @ScheduleId, @PenaltyAmount, @Status, @CreatedAt)";

            var rowsAffected = await _databaseService.ExecuteNonQueryAsync(sql,
                new SqlParameter("@LoanAccountId", penalty.LoanAccountId),
                new SqlParameter("@ScheduleId", penalty.LoanAccountId),
                new SqlParameter("@PenaltyAmount", penalty.LoanAccountId),
                new SqlParameter("@Status", penalty.LoanAccountId),
                new SqlParameter("@CreatedAt", penalty.LoanAccountId));


            return rowsAffected > 0;

        }


        public async Task<bool> UpdatePenaltyStatusAsync(int penaltyId, string status)
        {
            const string sql = @"UPDATE Penalties 
                SET Status = @Status
                WHERE PenaltyId = @PenaltyId";

            var rowsAffected = await _databaseService.ExecuteNonQueryAsync(sql,
                new SqlParameter("@Status", status),
                new SqlParameter("@PenaltyId", penaltyId));

            return rowsAffected > 0;
        }

        public async Task<decimal>GetTotalPenaltiesAsync(int LoanAccountId)
        {
            const string sql = @"SELECT ISNULL(SUM(PenaltyAmount), 0)
                FROM Penalties 
                WHERE LoanAccountId = @LoanAccountId AND Status = 'Unpaid'";

            var result = await _databaseService.ExecuteScalarAsync(sql,
                new SqlParameter("@LoanAccountId", LoanAccountId));
            return result !=null ? Convert.ToDecimal(result) : 0;
        }



    }
}
