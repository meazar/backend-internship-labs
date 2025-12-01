using LoanManagementSystem.Attitubes;
using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController : ControllerBase
    {
       
        private readonly IUserRepository _userRepository;
        private readonly ILoanRepository _loanRepository;

        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IUserRepository userRepository,
            ILoanRepository loanRepository,

            ILogger<AdminController> logger)
        {
            _userRepository = userRepository;
            _loanRepository = loanRepository;

            _logger = logger;
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetUserByRoleAsync("Customer");
                var userResponses = users.Select(u => new UserResponse
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.PhoneNumber,
                    Role = u.Role,
                    KYCStatus = u.KYCStatus,
                    DateOfBirth = u.DateOfBirth,
                    Address = u.Address,
                    CreatedAt = u.CreatedAt
                });

                return Ok(new { success = true, data = userResponses });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpGet("officers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOfficers()
        {
            try
            {
                var officers = await _userRepository.GetUserByRoleAsync("Officer");
                var officerResponses = officers.Select(u => new UserResponse
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.PhoneNumber,
                    Role = u.Role,
                    KYCStatus = u.KYCStatus,
                    DateOfBirth = u.DateOfBirth,
                    Address = u.Address,
                    CreatedAt = u.CreatedAt
                });

                return Ok(new { success = true, data = officerResponses });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all officers");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPut("kyc-verify/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateKYCStatus(int userId, [FromBody] UpdateKYCStatusRequest request)
        {
            try
            {
                var success = await _userRepository.UpdatedKYCStatusAsync(userId, request.Status);
                if (!success)
                {
                    return NotFound(new { message = "User not found" });
                }

                _logger.LogInformation("Admin updated KYC status for user {UserId} to {Status}",
                    userId, request.Status);

                return Ok(new { message = "KYC status updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating KYC status for user {UserId}", userId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
