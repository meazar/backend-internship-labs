using System.Security.Claims;
using LMS.Core.Entities;

namespace LMS.Core.Interfaces.Services;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    DateTime GetRefreshTokenExpiry();
}
