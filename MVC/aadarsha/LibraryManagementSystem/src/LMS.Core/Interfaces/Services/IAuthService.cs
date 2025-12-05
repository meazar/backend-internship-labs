using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;

namespace LMS.Core.Interfaces.Services;

public interface IAuthService
{
    Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<ApiResponse<bool>> LogoutAsync(int userId);
    Task<ApiResponse<bool>> ChangePasswordAsync(int userId, ChangePasswordRequest request);
    Task<ApiResponse<bool>> RevokeTokenAsync(int userId);
    Task<bool> ValidateTokenAsync(string accessToken);
    Task<int?> GetUserIdFromTokenAsync(string accessToken);
}
