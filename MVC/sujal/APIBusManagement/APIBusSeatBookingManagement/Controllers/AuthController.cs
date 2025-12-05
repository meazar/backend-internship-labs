using APIBusSeatBookingManagement.Database;
using APIBusSeatBookingManagement.Dto;
using APIBusSeatBookingManagement.Model;
using APIBusSeatBookingManagement.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIBusSeatBookingManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJWTService _jwtService;
        private readonly IPasswordService _passwordService;



        public AuthController(AppDbContext context,IJWTService jwtService,IPasswordService passwordService )
        {
            _context = context;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }


        [HttpPost("register")]

        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest("Email is Already Exists");
            }

            var user = new User
            {
                FullName = registerDto.Name,
                Email = registerDto.Email,
                Password = _passwordService.HashPassword(registerDto.Password),
                Role = registerDto.Role,

            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var token = _jwtService.GenerateToken(user);


            return Ok(new AuthResponseDto
            {
                UserId = user.UserId,
                UserName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = token

            });
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !_passwordService.VerifyPassword(loginDto.Password, user.Password))
            {
                return Unauthorized("Invaild  creadentials");
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new AuthResponseDto
            {
                UserId = user.UserId,
                UserName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            });
        }
    }
}
