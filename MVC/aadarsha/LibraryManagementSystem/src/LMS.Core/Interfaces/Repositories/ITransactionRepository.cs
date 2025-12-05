using LMS.Core.Entities;

namespace LMS.Core.Interfaces.Repositories;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<Transaction?> GetWithDetailsAsync(int id);
    Task<IEnumerable<Transaction>> GetByMemberIdAsync(int memberId);
    Task<IEnumerable<Transaction>> GetByBookIdAsync(int bookId);
    Task<IEnumerable<Transaction>> GetOverdueTransactionsAsync();
    Task<IEnumerable<Transaction>> GetActiveCheckoutsAsync(int memberId);
    Task<Transaction?> GetActiveCheckoutAsync(int memberId, int bookId);
    Task<int> GetActiveCheckoutCountAsync(int memberId);
    Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(
        DateTime startDate,
        DateTime endDate
    );
}
