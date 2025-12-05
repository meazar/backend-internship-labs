using System.Security.Claims;
using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;
using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using LMS.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IJwtService jwtService,
        ILogger<AuthService> logger
    )
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                return ApiResponse<AuthResponse>.ErrorResponse("Email already exists.");
            }

            if (await _userRepository.UsernameExistsAsync(request.Username))
            {
                return ApiResponse<AuthResponse>.ErrorResponse("Username already exists.");
            }

            if (!IsValidRole(request.Role))
            {
                return ApiResponse<AuthResponse>.ErrorResponse("Invalid role specified.");
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                Password = HashPassword(request.Password),
                Role = request.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiry = _jwtService.GetRefreshTokenExpiry();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = refreshTokenExpiry;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation("User registered: {Email}", user.Email);

            var response = new AuthResponse
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60),
                RefreshTokenExpiry = refreshTokenExpiry,
            };

            return ApiResponse<AuthResponse>.SuccessResponse(response, "Registration successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
            return ApiResponse<AuthResponse>.ErrorResponse(
                "An error occurred during registration."
            );
        }
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return ApiResponse<AuthResponse>.ErrorResponse("Invalid email or password.");
            }

            if (!user.IsActive)
            {
                return ApiResponse<AuthResponse>.ErrorResponse("Account is deactivated.");
            }

            if (!VerifyPassword(request.Password, user.Password))
            {
                return ApiResponse<AuthResponse>.ErrorResponse("Invalid email or password.");
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiry = _jwtService.GetRefreshTokenExpiry();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = refreshTokenExpiry;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation("User logged in: {Email}", user.Email);

            var response = new AuthResponse
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60),
                RefreshTokenExpiry = refreshTokenExpiry,
            };

            return ApiResponse<AuthResponse>.SuccessResponse(response, "Login successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
            return ApiResponse<AuthResponse>.ErrorResponse("An error occurred during login.");
        }
    }

    public async Task<ApiResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        try
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return ApiResponse<TokenResponse>.ErrorResponse("Invalid access token.");
            }

            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _userRepository.GetByIdAsync(userId);

            if (
                user == null
                || user.RefreshToken != request.RefreshToken
                || user.RefreshTokenExpiry <= DateTime.UtcNow
            )
            {
                return ApiResponse<TokenResponse>.ErrorResponse("Invalid refresh token.");
            }

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiry = _jwtService.GetRefreshTokenExpiry();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = refreshTokenExpiry;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            var response = new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60),
            };

            return ApiResponse<TokenResponse>.SuccessResponse(
                response,
                "Token refreshed successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return ApiResponse<TokenResponse>.ErrorResponse(
                "An error occurred while refreshing token."
            );
        }
    }

    public async Task<ApiResponse<bool>> LogoutAsync(int userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.ErrorResponse("User not found.");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation("User logged out: {UserId}", userId);
            return ApiResponse<bool>.SuccessResponse(true, "Logout successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout for user: {UserId}", userId);
            return ApiResponse<bool>.ErrorResponse("An error occurred during logout.");
        }
    }

    public async Task<ApiResponse<bool>> ChangePasswordAsync(
        int userId,
        ChangePasswordRequest request
    )
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.ErrorResponse("User not found.");
            }

            if (!VerifyPassword(request.CurrentPassword, user.Password))
            {
                return ApiResponse<bool>.ErrorResponse("Current password is incorrect.");
            }

            user.Password = HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation("Password changed for user: {UserId}", userId);
            return ApiResponse<bool>.SuccessResponse(true, "Password changed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user: {UserId}", userId);
            return ApiResponse<bool>.ErrorResponse("An error occurred while changing password.");
        }
    }

    public async Task<ApiResponse<bool>> RevokeTokenAsync(int userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.ErrorResponse("User not found.");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation("Token revoked for user: {UserId}", userId);
            return ApiResponse<bool>.SuccessResponse(true, "Token revoked successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking token for user: {UserId}", userId);
            return ApiResponse<bool>.ErrorResponse("An error occurred while revoking token.");
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);
            if (principal == null)
                return false;

            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _userRepository.GetByIdAsync(userId);

            return user != null && user.IsActive;
        }
        catch
        {
            return false;
        }
    }

    public async Task<int?> GetUserIdFromTokenAsync(string token)
    {
        try
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);
            if (principal == null)
                return null;

            return int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }
        catch
        {
            return null;
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        var hash = HashPassword(password);
        return hash == hashedPassword;
    }

    private bool IsValidRole(string role)
    {
        var validRoles = new[] { "Admin", "Librarian", "Member" };
        return validRoles.Contains(role);
    }
}
