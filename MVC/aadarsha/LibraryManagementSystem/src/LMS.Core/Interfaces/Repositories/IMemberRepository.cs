using LMS.Core.Entities;

namespace LMS.Core.Interfaces.Repositories;

public interface IMemberRepository : IGenericRepository<Member>
{
    Task<Member?> GetByUserIdAsync(int userId);
    Task<Member?> GetByMemberIdAsync(string memberId);
    Task<Member?> GetWithDetailsAsync(int id);
    Task<IEnumerable<Member>> GetActiveMembersAsync();
    Task<IEnumerable<Member>> GetMembersWithFinesAsync();
    Task<bool> IsMemberIdExistsAsync(string memberId);
    Task<int> GetCheckedOutCountAsync(int memberId);
}
