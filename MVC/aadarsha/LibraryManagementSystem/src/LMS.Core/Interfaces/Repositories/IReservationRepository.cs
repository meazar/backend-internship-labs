using LMS.Core.Entities;

namespace LMS.Core.Interfaces.Repositories;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    Task<Reservation?> GetWithDetailsAsync(int id);
    Task<IEnumerable<Reservation>> GetByMemberIdAsync(int memberId);
    Task<IEnumerable<Reservation>> GetByBookIdAsync(int bookId);
    Task<IEnumerable<Reservation>> GetActiveReservationsAsync(int bookId);
    Task<IEnumerable<Reservation>> GetPendingReservationsAsync();
    Task<int> GetQueuePositionAsync(int bookId, int reservationId);
    Task<bool> HasActiveReservationAsync(int memberId, int bookId);
}
