using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;

namespace LMS.Core.Interfaces.Services;

public interface IUserService
{
    Task<ApiResponse<UserResponse>> CreateUserAsync(RegisterRequest request);
    Task<ApiResponse<UserResponse>> GetUserByIdAsync(int userId);
    Task<ApiResponse<IEnumerable<UserResponse>>> GetAllUsersAsync(
        int pageNumber = 1,
        int pageSize = 10
    );
    Task<ApiResponse<UserResponse>> UpdateUserAsync(int id, UpdateUserRequest request);
    Task<ApiResponse<bool>> DeleteUserAsync(int id);
    Task<ApiResponse<bool>> ActivateUserAsync(int id);
    Task<ApiResponse<bool>> DeactivateUserAsync(int id);
}
