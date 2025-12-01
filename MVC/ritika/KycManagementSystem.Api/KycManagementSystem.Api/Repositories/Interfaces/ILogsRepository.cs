using KycManagementSystem.Api.Models.DTOs.Pagination;
using KycManagementSystem.Api.Models.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KycManagementSystem.Api.Repositories.Interfaces
{
    public interface ILogsRepository
    {
        Task<int> LogAsync(ApiLog log);
        Task<IEnumerable<ApiLog>> GetAllLogsAsync();
        Task<PageResult<ApiLog>> GetPagedLogsAsync(PageRequest request);
    }
}
