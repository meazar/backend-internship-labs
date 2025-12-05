using LoanManagementSystem.Models;


namespace LoanManagementSystem.Service
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(CreateUserRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        
    }
}
