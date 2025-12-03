using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdoptionSystemApi.DTOs.User;
using PetAdoptionSystemApi.Models;
using PetAdoptionSystemApi.Services.Interfaces;

namespace PetAdoptionSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(_mapper.Map<IEnumerable<UserResponseDto>>(users));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponseDto>> Get(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserResponseDto>(user));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create(UserCreateDto dto)
        {
            var user = _mapper.Map<User>(dto);
            await _userService.CreateAsync(user, dto.Password);
            var response = _mapper.Map<UserResponseDto>(user);
            return CreatedAtAction(nameof(Get), new { id = response.UserId }, response);
        }
        
    }
}
