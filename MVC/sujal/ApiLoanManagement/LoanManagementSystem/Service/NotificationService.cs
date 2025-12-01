using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;

namespace LoanManagementSystem.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task NotifyLoanApplicationSubmitted(int userId, int applicationId)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = "Loan Application Submited",
                Message = $"Your loan application #{applicationId} has been submited successfully and is under review.",
                Type = "App",
                IsRead = false,
                CreatedAt = DateTime.UtcNow,

            };

            await _notificationRepository.CreateNotificationAsync(notification);
        }


        public async Task NotifyLoanApplicationStatus(int userId, int applicationId,string status)
        {
            var message = status == "Approved"
                ? $"Congratulation! YOUr Loan application #{applicationId} has been approved."
                : $"Your loan application #{applicationId} has been {status.ToLower()}.";

            var notification = new Notification
            {
                UserId = userId,
                Title = $"Loan Application {status}",
                Message = message,
                Type = "APP",
                IsRead = false,
                CreatedAt = DateTime.UtcNow,

            };
            await _notificationRepository.CreateNotificationAsync (notification);

        }

        public async Task NotifyPaymentDue(int userId, int loanAccount, int scheduleId)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = "Payment due Reminder",
                Message = $"Your EMI Payment for loan account #{loanAccount} is deo soon. please make the payment to voidd late fees.",
                Type = "APP",
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
            };
            await _notificationRepository.CreateNotificationAsync(notification);
        }




        public async Task NotifyPaymentReceived(int userId, int loanAccountId, decimal amount)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = "Payment Received",
                Message = $"Payment of {amount:C} for loan account #{loanAccountId} has been received successfully.",
                Type = "App",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _notificationRepository.CreateNotificationAsync(notification);
        }
        public async Task NotifyKYCAprovalStatus (int userId, string status)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = "KYC Verification Status",
                Message = $"Your KYC verification has been {status.ToLower()}.",
                Type = "App",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.CreateNotificationAsync(notification);
        }
    }


}
