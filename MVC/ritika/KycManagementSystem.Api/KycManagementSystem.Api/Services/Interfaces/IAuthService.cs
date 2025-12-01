using KycManagementSystem.Api.Models.DTOs.Auth;

namespace KycManagementSystem.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<int> RegisterAsync(string username, string email, string password, string role);
        Task<AuthResponse> LoginAsync(string username, string password);
    }
}
