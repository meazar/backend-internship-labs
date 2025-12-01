using LoanManagementSystem.Models;

namespace LoanManagementSystem.IRepository
{
    public interface IAuditRepository
    {
        Task<int> LogActionAsync(AuditLog auditLog);
        Task<IEnumerable<AuditLog>> GetAuditLogsAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<IEnumerable<AuditLog>> GetAuditLogsByUserAsync(int userId);
        Task<IEnumerable<AuditLog>> GetAuditLogsByTableAsync(string tableName);
    }
}
