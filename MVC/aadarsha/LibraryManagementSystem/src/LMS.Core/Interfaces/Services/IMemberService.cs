using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;

namespace LMS.Core.Interfaces.Services;

public interface IMemberService
{
    Task<ApiResponse<MemberResponse>> CreateMemberAsync(int userId);
    Task<ApiResponse<MemberResponse>> GetMemberByIdAsync(int id);
    Task<ApiResponse<MemberResponse>> GetMemberByUserIdAsync(int userId);
    Task<ApiResponse<MemberResponse>> GetMemberByMemberIdAsync(string memberId);
    Task<ApiResponse<IEnumerable<MemberResponse>>> GetAllMembersAsync();
    Task<ApiResponse<IEnumerable<MemberResponse>>> GetActiveMembersAsync();
    Task<ApiResponse<MemberResponse>> UpdateMemberAsync(int id, UpdateMemberRequest request);
    Task<ApiResponse<bool>> DeactivateMemberAsync(int id);
    Task<ApiResponse<bool>> ActivateMemberAsync(int id);
    Task<ApiResponse<MemberStatusResponse>> GetMemberStatusAsync(int memberId);
    Task<ApiResponse<bool>> CanBorrowMoreBooksAsync(int memberId);
}
