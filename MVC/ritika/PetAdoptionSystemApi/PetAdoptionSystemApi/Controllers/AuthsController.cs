using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetAdoptionSystemApi.Data;
using PetAdoptionSystemApi.DTOs.Auth;
using PetAdoptionSystemApi.Models;
using PetAdoptionSystemApi.utils;


namespace PetAdoptionSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtService _jwt;

        public AuthsController(ApplicationDbContext db, JwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if(await _db.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest(new { error = "Email already exists." });
              
            }
            
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                Role = Role.Adopter,
                Password =PasswordHasher.Hash(dto.Password)
            };
           
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            var token = _jwt.GenerateToken(user);
            return Ok(new { token });

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if(user == null)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }
            bool verified = PasswordHasher.Verify(dto.Password, user.Password);
            if (!verified)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }
            var token = _jwt.GenerateToken(user);
            return Ok(new { token });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u=> u.Email == dto.Email))
            {
                return BadRequest(new { error = "Email already exists" });

            }
            var adminUser = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Role = Role.Admin,
                Address = dto.Address,
                Password = PasswordHasher.Hash(dto.Password),
                Phone = dto.Phone
            };
            _db.Users.Add(adminUser);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Admin created Successfully" });
        }

    }
}
