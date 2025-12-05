using System.Security.Cryptography;
using System.Text;
using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;
using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using LMS.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<UserService> logger
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiResponse<UserResponse>> CreateUserAsync(RegisterRequest req)
    {
        try
        {
            if (await _userRepository.UsernameExistsAsync(req.Username))
            {
                return ApiResponse<UserResponse>.ErrorResponse("Username already exists.");
            }
            if (await _userRepository.EmailExistsAsync(req.Email))
            {
                return ApiResponse<UserResponse>.ErrorResponse("Email already exists.");
            }

            var passwordHash = HashPassword(req.Password);

            var user = new User
            {
                Username = req.Username,
                Email = req.Email,
                Password = passwordHash,
                FirstName = req.FirstName ?? string.Empty,
                LastName = req.LastName ?? string.Empty,
                Role = req.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await _userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("User created with ID: {UserId}", user.Id);

            var response = MapToResponse(user);
            return ApiResponse<UserResponse>.SuccessResponse(
                response,
                "User created successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {ErrorMessage}", ex.Message);
            return ApiResponse<UserResponse>.ErrorResponse(
                "An error occurred while creating user."
            );
        }
    }

    public async Task<ApiResponse<UserResponse>> GetUserByIdAsync(int userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<UserResponse>.ErrorResponse("User not found.");
            }

            var response = MapToResponse(user);
            return ApiResponse<UserResponse>.SuccessResponse(
                response,
                "User retrieved successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user: {ErrorMessage}", ex.Message);
            return ApiResponse<UserResponse>.ErrorResponse(
                "An error occurred while retrieving user."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<UserResponse>>> GetAllUsersAsync(
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        try
        {
            var users = await _userRepository.GetPagedAsync(pageNumber, pageSize);
            var userResponses = users.Select(MapToResponse);

            return ApiResponse<IEnumerable<UserResponse>>.SuccessResponse(
                userResponses,
                "Users retrieved successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users: {ErrorMessage}", ex.Message);
            return ApiResponse<IEnumerable<UserResponse>>.ErrorResponse(
                "An error occurred while retrieving users."
            );
        }
    }

    public async Task<ApiResponse<UserResponse>> UpdateUserAsync(int id, UpdateUserRequest req)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<UserResponse>.ErrorResponse("User not found.");
            }

            if (!string.IsNullOrWhiteSpace(req.FirstName))
                user.FirstName = req.FirstName;

            if (!string.IsNullOrWhiteSpace(req.LastName))
                user.LastName = req.LastName;

            user.IsActive = req.IsActive;

            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            var response = MapToResponse(user);
            return ApiResponse<UserResponse>.SuccessResponse(
                response,
                "User updated successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {userId}: {ErrorMessage}", id, ex.Message);
            return ApiResponse<UserResponse>.ErrorResponse(
                "An error occurred while updating user."
            );
        }
    }

    public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<bool>.ErrorResponse("User not found.");
            }

            await _userRepository.DeleteAsync(user);
            await _unitOfWork.CompleteAsync();

            // soft delete: deactivate
            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.SuccessResponse(true, "User deactivated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {userId}: {ErrorMessage}", id, ex.Message);
            return ApiResponse<bool>.ErrorResponse("An error occurred while deleting user.");
        }
    }

    public async Task<ApiResponse<bool>> ActivateUserAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<bool>.ErrorResponse("User not found.");
            }

            // activate user
            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.SuccessResponse(true, "User activated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating user {userId}: {ErrorMessage}", id, ex.Message);
            return ApiResponse<bool>.ErrorResponse("An error occurred while activating user.");
        }
    }

    public async Task<ApiResponse<bool>> DeactivateUserAsync(int id)
    {
        return await DeleteUserAsync(id); // same as soft delete
    }

    // Helper methods: HashPassword and MapToResponse
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id.GetHashCode(),
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
        };
    }
}
