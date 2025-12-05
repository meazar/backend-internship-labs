namespace LoanManagementSystem.Service
{
    public interface INotificationService
    {
        Task NotifyLoanApplicationSubmitted(int userId, int applicationId);
        Task NotifyLoanApplicationStatus(int userId, int applicationId, string status);
        Task NotifyPaymentDue(int userId, int loanAccountId, int scheduleId);
        Task NotifyPaymentReceived(int userId, int loanAccountId, decimal amount);
        Task NotifyKYCAprovalStatus(int userId, string status);
    }
}
