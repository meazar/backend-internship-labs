using LoanManagementSystem.Models;
using System.Security.Claims;

namespace LoanManagementSystem.Service
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
        string GetUserIdFromToken(string token);
        string GetUserRoleFromToken(string token);
    }
}
