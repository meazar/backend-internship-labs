using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data.Repositories;

public class MemberRepository : GenericRepository<Member>, IMemberRepository
{
    public MemberRepository(LMSDbContext context)
        : base(context) { }

    public async Task<Member?> GetByUserIdAsync(int userId)
    {
        return await _dbSet.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId);
    }

    public async Task<Member?> GetByMemberIdAsync(string memberId)
    {
        return await _dbSet.Include(m => m.User).FirstOrDefaultAsync(m => m.MemberId == memberId);
    }

    public async Task<Member?> GetWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(m => m.User)
            .Include(m => m.Transactions)
                .ThenInclude(t => t.Book)
            .Include(m => m.Reservations)
            .Include(m => m.Fines)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Member>> GetActiveMembersAsync()
    {
        return await _dbSet
            .Include(m => m.User)
            .Where(m => m.IsMembershipActive && m.User.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Member>> GetMembersWithFinesAsync()
    {
        return await _dbSet
            .Include(m => m.User)
            .Include(m => m.Fines.Where(f => f.Status == "Pending" && f.IsActive))
            .Where(m => m.Fines.Any(f => f.Status == "Pending" && f.IsActive))
            .ToListAsync();
    }

    public async Task<bool> IsMemberIdExistsAsync(string memberId)
    {
        return await _dbSet.AnyAsync(m => m.MemberId == memberId);
    }

    public async Task<int> GetCheckedOutCountAsync(int memberId)
    {
        return await _context.Transactions.CountAsync(t =>
            t.MemberId == memberId && t.Status == "CheckedOut"
        );
    }
}
