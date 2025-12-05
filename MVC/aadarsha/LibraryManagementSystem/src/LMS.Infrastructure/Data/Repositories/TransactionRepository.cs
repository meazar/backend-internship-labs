using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(LMSDbContext context)
        : base(context) { }

    public async Task<Transaction?> GetWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(t => t.Member)
                .ThenInclude(m => m.User)
            .Include(t => t.Book)
            .Include(t => t.Fine)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Transaction>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Include(t => t.Book)
            .Include(t => t.Fine)
            .Where(t => t.MemberId == memberId)
            .OrderByDescending(t => t.CheckoutDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByBookIdAsync(int bookId)
    {
        return await _dbSet
            .Include(t => t.Member)
                .ThenInclude(m => m.User)
            .Where(t => t.BookId == bookId)
            .OrderByDescending(t => t.CheckoutDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetOverdueTransactionsAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Include(t => t.Member)
                .ThenInclude(m => m.User)
            .Include(t => t.Book)
            .Where(t => t.Status == "CheckedOut" && t.DueDate < now && t.ReturnDate == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetActiveCheckoutsAsync(int memberId)
    {
        return await _dbSet
            .Include(t => t.Book)
            .Where(t => t.MemberId == memberId && t.Status == "CheckedOut" && t.ReturnDate == null)
            .ToListAsync();
    }

    public async Task<Transaction?> GetActiveCheckoutAsync(int memberId, int bookId)
    {
        return await _dbSet.FirstOrDefaultAsync(t =>
            t.MemberId == memberId
            && t.BookId == bookId
            && t.Status == "CheckedOut"
            && t.ReturnDate == null
        );
    }

    public async Task<int> GetActiveCheckoutCountAsync(int memberId)
    {
        return await _dbSet.CountAsync(t =>
            t.MemberId == memberId && t.Status == "CheckedOut" && t.ReturnDate == null
        );
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(
        DateTime startDate,
        DateTime endDate
    )
    {
        return await _dbSet
            .Include(t => t.Member)
                .ThenInclude(m => m.User)
            .Include(t => t.Book)
            .Where(t => t.CheckoutDate >= startDate && t.CheckoutDate <= endDate)
            .OrderByDescending(t => t.CheckoutDate)
            .ToListAsync();
    }
}
