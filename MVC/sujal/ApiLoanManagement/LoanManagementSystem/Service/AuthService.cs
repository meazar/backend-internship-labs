using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;


namespace LoanManagementSystem.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;


        public AuthService(IUserRepository userRepository, IJwtService jwtService, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<AuthResponse> RegisterAsync(CreateUserRequest request)
        {
            try
            {
                // Validate email uniqueness
                var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "User with this email already exists"
                    };
                }

                // Validate role
                if (!IsValidRole(request.Role))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid role specified"
                    };
                }

                var userId = await _userRepository.CreateUserAsync(request);
                if (userId == 0)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Failed to create user"
                    };
                }

                // Get the created user
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "User created but cannot retrieve details"
                    };
                }

                // Generate token
                var token = _jwtService.GenerateToken(user);

                _logger.LogInformation("User registered successfully: {Email}, Role: {Role}",
                    user.Email, user.Role);

                return new AuthResponse
                {
                    Success = true,
                    Message = "User registered successfully",
                    Token = token,
                    User = MapToUserResponse(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration for {Email}", request.Email);
                return new AuthResponse
                {
                    Success = false,
                    Message = "Internal server error during registration"
                };
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                // Find user by email
                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                // Check if user is active (you might want to add an IsActive field)
                if (user.KYCStatus == "Rejected")
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Your account has been rejected. Please contact support."
                    };
                }

                // Generate token
                var token = _jwtService.GenerateToken(user);

                _logger.LogInformation("User logged in successfully: {Email}, Role: {Role}",
                    user.Email, user.Role);

                return new AuthResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    User = MapToUserResponse(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login for {Email}", request.Email);
                return new AuthResponse
                {
                    Success = false,
                    Message = "Internal server error during login"
                };
            }
        }
        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null || !BCrypt.Net.BCrypt.Verify(currentPassword, user.Password))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return false;
                }

                // Reset password logic would go here
                // You'll need to add this method to IUserRepository
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for {Email}", email);
                return false;
            }
        }

        private bool IsValidRole(string role)
        {
            return role == "Customer" || role == "Officer" || role == "Admin";
        }

        private UserResponse MapToUserResponse(User user)
        {
            return new UserResponse
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
            };
        }
    }
        
    
}
