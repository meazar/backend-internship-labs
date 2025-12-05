using LoanManagementSystem.Models;

namespace LoanManagementSystem.IRepository
{
    public interface IPaymentRepository
    {
        Task<int> CreatePaymentScheduleAsync(PaymentSchedule schedule);
        Task<IEnumerable<PaymentSchedule>> GetPaymentScheduleByLoanAccountAsync(int loanAccountId);
        Task<PaymentSchedule?> GetPaymentScheduleByIdAsync(int scheduleId);
        Task<bool> UpdatePaymentScheduleStatusAsync(int scheduleId, string status, DateTime paidDate);
        Task<int> MakePaymentAsync(Payments payment);
        Task<IEnumerable<Payments>> GetPaymentsByLoanAccountAsync(int loanAccountId);
        Task<bool> AddPenaltyAsync(Penalty penalty);
        Task<bool> UpdatePenaltyStatusAsync(int penaltyId, string status);
        Task<decimal> GetTotalPenaltiesAsync(int loanAccountId);
    }
}
