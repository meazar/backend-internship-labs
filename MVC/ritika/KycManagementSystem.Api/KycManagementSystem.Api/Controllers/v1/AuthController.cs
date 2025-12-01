using KycManagementSystem.Api.Models.DTOs.Auth;
using KycManagementSystem.Api.Services.Interfaces;
using KycManagementSystem.Api.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KycManagementSystem.Api.Controllers.v1
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request.Username, request.Password);

            if (response!=null)
            {
                var commonResponse = new NewApiResponse<AuthResponse>
                {
                    code = "200",
                    Message = "Login Successfully",
                    Data = response
                };
                // return Ok(ApiResponse<string>.Ok("Login Successfully"));
                return Ok(commonResponse);
            }
            else
            {
                 var commonResponse = new NewApiResponse<AuthResponse>
                {
                    code = "401",
                    Message = "Invalid credentials"
                };
                return Unauthorized(commonResponse);


            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(
                request.Username,
                request.Email,
                request.Password,
                request.Role
            );
            if (result <= 0)
                return BadRequest(ApiResponse<string>.Fail("Register Failed"));

            return Ok(ApiResponse<string>.Ok("Registered Successfully"));

        }
    }
}
