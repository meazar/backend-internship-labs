using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KycManagementSystem.Api.Models.DTOs.Auth;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;
using KycManagementSystem.Api.Services.Interfaces;
using KycManagementSystem.Api.utils;
using Microsoft.IdentityModel.Tokens;


namespace KycManagementSystem.Api.Services.Implementations
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }
        public async Task<int> RegisterAsync(string username,string email, string password, string role)
        {
            var exists = await _userRepo.GetByUsernameAsync(username);
            if (exists != null)
            {
                throw new Exception("Username exists");
            }
            var user = new User
            {
                Username = username,
                Email = email,
                Password = PasswordHasher.Hash(password),
                Role = role,
                IsActive=true
            };
            return await _userRepo.CreateUserAsync(user);
        }
        public async Task<AuthResponse?> LoginAsync(string username, string password)
        {
            var user = await _userRepo.GetByUsernameAsync(username);
            if (user == null || !PasswordHasher.Verify(password, user.Password))
                return null;

            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim("role", user.Role),
            new Claim("id", user.Id.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds
            );

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = user.Username,
                Role = user.Role
            };
        }

    }
}
