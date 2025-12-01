/*using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;


        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }


        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);

                if (existingUser != null)
                {
                    return BadRequest(new { message = "USer with this email already exists...." });
                }

                var userId = await _userRepository.CreateUserAsync(request);

                if (userId > 0)
                {
                    return Ok(new { message = "User Register successfully....", userId });
                }


                return StatusCode(500, new { message = "Failed to create user" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(request.Email);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invaild email or Password.." });
                }
                return Ok(new
                {
                    message = "Login SuccessFul",
                    user = new
                    {
                        user.UserId,
                        user.FullName,
                        user.Email,
                        user.Role,
                        user.KYCStatus,
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new { message = "internal Server Error" });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if(user == null)
                {
                    return NotFound(new { message = "User Not Found" });

                }

                return Ok(new
                {
                    user.UserId,
                    user.FullName,
                    user.Email,
                    user.PhoneNumber,
                    user.Role,
                    user.KYCStatus,
                    user.DateOfBirth,
                    user.Address,
                    user.CreatedAt

                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in getting user");
                return StatusCode(500, new { message = "Internal Server is Failed" });
            }
        }


        [HttpPut("{userId}/kyc-status")]
        public async Task<IActionResult> UpdateKYCStatus(int userId, [FromBody] UpdateKYCStatusRequest request)
        {
            try
            {
                var success = await _userRepository.UpdatedKYCStatusAsync(userId, request.Status);
                if (!success)
                {
                    return NotFound(new { message = "USer NOT FOUND" });

                }
                return Ok(new { message = "KYC Status updated successfully.." });

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating KYC Status");
                return StatusCode(500, new { message = "Internal server error" });

            }

        }


        public class UpdateKYCStatusRequest
        {
            public string Status { get; set; } 
        }
    }
}
*/