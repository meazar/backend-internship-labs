using LoanManagementSystem.Attitubes;
using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;
        public AuthController(IAuthService authService, ILogger<AuthController> logger, IUserRepository userRepository)
        {
            _authService = authService;
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            try
            {
                // Default to Customer role for public registration
                if (string.IsNullOrEmpty(request.Role))
                {
                    request.Role = "Customer";
                }

                // Only allow Customer role for public registration
                if (request.Role != "Customer")
                {
                    return BadRequest(new { message = "Only Customer role can register publicly" });
                }

                var result = await _authService.RegisterAsync(request);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in user registration");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("register-officer")]
        [Authorize(Roles ="Admin")]
        
        public async Task<IActionResult> RegisterOfficer([FromBody] CreateUserRequest request)
        {
            try
            {
                // Force Officer role for admin-created accounts
                request.Role = "Officer";

                var result = await _authService.RegisterAsync(request);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in officer registration");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpPost("register-admin")]
        [Authorize(Roles ="Admin")]
       
        public async Task<IActionResult> RegisterAdmin([FromBody] CreateUserRequest request)
        {
            try
            {
                // Force Admin role
                request.Role = "Admin";

                var result = await _authService.RegisterAsync(request);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in admin registration");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("login")]
        
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {

                var result = await _authService.LoginAsync(request);

                if (result.Success)
                {
                    return Ok(result);
                }

                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in user login");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
        [HttpPost("change-password")]
        
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                var success = await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);

                if (success)
                {
                    return Ok(new { message = "Password changed successfully" });
                }

                return BadRequest(new { message = "Failed to change password" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("profile")]
        
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                var user = await _userRepository.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new UserResponse
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Role = user.Role,
                    KYCStatus = user.KYCStatus,
                    DateOfBirth = user.DateOfBirth,
                    Address = user.Address,
                    CreatedAt = user.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}

