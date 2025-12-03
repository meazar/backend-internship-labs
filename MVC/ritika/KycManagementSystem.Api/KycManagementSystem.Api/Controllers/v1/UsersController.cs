using KycManagementSystem.Api.Models.DTOs.Pagination;
using KycManagementSystem.Api.Models.DTOs.Users;
using KycManagementSystem.Api.Services.Interfaces;
using KycManagementSystem.Api.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KycManagementSystem.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var id = await _userService.CreateUserAsync(dto);
            return Ok(ApiResponse<int>.Ok(id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(ApiResponse<UserDto>.Ok(user));
        }

        [HttpPost("paged")]
        public async Task<IActionResult> GetPagedUsers([FromBody] PageRequest request)
        {
            var result = await _userService.GetPagedUsersAsync(request);
            return Ok(ApiResponse<PageResult<UserDto>>.Ok(result));
        }

    }
}
