using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;

namespace LMS.Core.Interfaces.Services;

public interface ITransactionService
{
    Task<ApiResponse<TransactionResponse>> CheckoutBookAsync(CheckoutRequest request);
    Task<ApiResponse<TransactionResponse>> ReturnBookAsync(int transactionId);
    Task<ApiResponse<TransactionResponse>> RenewBookAsync(int transactionId);
    Task<ApiResponse<TransactionResponse>> GetTransactionByIdAsync(int id);
    Task<ApiResponse<IEnumerable<TransactionResponse>>> GetTransactionsByMemberAsync(int memberId);
    Task<ApiResponse<IEnumerable<TransactionResponse>>> GetTransactionsByBookAsync(int bookId);
    Task<ApiResponse<IEnumerable<TransactionResponse>>> GetOverdueTransactionsAsync();
    Task<ApiResponse<IEnumerable<TransactionResponse>>> GetActiveCheckoutsAsync(int memberId);
    Task<ApiResponse<CheckoutEligibilityResponse>> CheckCheckoutEligibilityAsync(
        int memberId,
        int bookId
    );
    Task<ApiResponse<TransactionStatsResponse>> GetTransactionStatsAsync(
        DateTime startDate,
        DateTime endDate
    );
}
