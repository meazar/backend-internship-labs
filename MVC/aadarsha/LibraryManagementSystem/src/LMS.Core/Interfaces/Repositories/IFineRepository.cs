using LMS.Core.Entities;

namespace LMS.Core.Interfaces.Repositories;

public interface IFineRepository : IGenericRepository<Fine>
{
    Task<Fine?> GetWithDetailsAsync(int id);
    Task<IEnumerable<Fine>> GetByMemberIdAsync(int memberId);
    Task<IEnumerable<Fine>> GetPendingFinesAsync();
    Task<IEnumerable<Fine>> GetOverdueFinesAsync();
    Task<decimal> GetTotalPendingFinesAsync(int memberId);
    Task<bool> HasPendingFinesAsync(int memberId);
}
