using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(LMSDbContext context)
        : base(context) { }

    public async Task<Reservation?> GetWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(r => r.Member)
                .ThenInclude(m => m.User)
            .Include(r => r.Book)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Reservation>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Include(r => r.Book)
            .Where(r => r.MemberId == memberId)
            .OrderByDescending(r => r.ReservationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByBookIdAsync(int bookId)
    {
        return await _dbSet
            .Include(r => r.Member)
                .ThenInclude(m => m.User)
            .Where(r => r.BookId == bookId)
            .OrderBy(r => r.PositionInQueue)
            .ThenBy(r => r.ReservationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync(int bookId)
    {
        return await _dbSet
            .Include(r => r.Member)
                .ThenInclude(m => m.User)
            .Where(r => r.BookId == bookId && (r.Status == "Pending" || r.Status == "Available"))
            .OrderBy(r => r.PositionInQueue)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetPendingReservationsAsync()
    {
        return await _dbSet
            .Include(r => r.Member)
                .ThenInclude(m => m.User)
            .Include(r => r.Book)
            .Where(r => r.Status == "Pending")
            .OrderBy(r => r.ReservationDate)
            .ToListAsync();
    }

    public async Task<int> GetQueuePositionAsync(int bookId, int reservationId)
    {
        var reservations = await _dbSet
            .Where(r => r.BookId == bookId && (r.Status == "Pending" || r.Status == "Available"))
            .OrderBy(r => r.PositionInQueue)
            .ToListAsync();

        var position = reservations.FindIndex(r => r.Id == reservationId);
        return position >= 0 ? position + 1 : 0;
    }

    public async Task<bool> HasActiveReservationAsync(int memberId, int bookId)
    {
        return await _dbSet.AnyAsync(r =>
            r.MemberId == memberId
            && r.BookId == bookId
            && (r.Status == "Pending" || r.Status == "Available")
        );
    }
}
