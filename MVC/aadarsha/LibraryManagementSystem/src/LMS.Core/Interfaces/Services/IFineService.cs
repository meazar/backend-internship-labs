using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;

namespace LMS.Core.Interfaces.Services;

public interface IFineService
{
    Task<ApiResponse<FineResponse>> CreateFineAsync(CreateFineRequest request);
    Task<ApiResponse<FineResponse>> GetFineByIdAsync(int id);
    Task<ApiResponse<IEnumerable<FineResponse>>> GetFinesByMemberAsync(int memberId);
    Task<ApiResponse<IEnumerable<FineResponse>>> GetPendingFinesAsync();
    Task<ApiResponse<FineResponse>> PayFineAsync(int fineId, decimal amount);
    Task<ApiResponse<FineResponse>> WaiveFineAsync(int fineId, string reason);
    Task<ApiResponse<decimal>> GetTotalPendingFinesAsync(int memberId);
    Task<ApiResponse<bool>> GenerateOverdueFinesAsync();
    Task<ApiResponse<FineSummaryResponse>> GetFineSummaryAsync(int memberId);
}
