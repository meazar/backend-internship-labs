using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data.Repositories;

public class FineRepository : GenericRepository<Fine>, IFineRepository
{
    public FineRepository(LMSDbContext context)
        : base(context) { }

    public async Task<Fine?> GetWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(f => f.Member)
                .ThenInclude(m => m.User)
            .Include(f => f.Transaction)
                .ThenInclude(t => t!.Book)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<Fine>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Include(f => f.Transaction)
                .ThenInclude(t => t!.Book)
            .Where(f => f.MemberId == memberId)
            .OrderByDescending(f => f.IssueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Fine>> GetPendingFinesAsync()
    {
        return await _dbSet
            .Include(f => f.Member)
                .ThenInclude(m => m.User)
            .Include(f => f.Transaction)
                .ThenInclude(t => t!.Book)
            .Where(f => f.Status == "Pending" && f.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Fine>> GetOverdueFinesAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Include(f => f.Member)
                .ThenInclude(m => m.User)
            .Where(f =>
                f.Status == "Pending" && f.DueDate.HasValue && f.DueDate < now && f.IsActive
            )
            .ToListAsync();
    }

    public async Task<decimal> GetTotalPendingFinesAsync(int memberId)
    {
        return await _dbSet
            .Where(f => f.MemberId == memberId && f.Status == "Pending" && f.IsActive)
            .SumAsync(f => f.Amount);
    }

    public async Task<bool> HasPendingFinesAsync(int memberId)
    {
        return await _dbSet.AnyAsync(f =>
            f.MemberId == memberId && f.Status == "Pending" && f.IsActive
        );
    }
}
