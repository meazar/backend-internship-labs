using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomerSupport.API.DTOs.Auth;
using CustomerSupport.API.Models;
using CustomerSupport.API.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace CustomerSupport.API.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _config = config;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetUserByEmail(dto.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials.");

        if (!PasswordHasher.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        return GenerateToken(user);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Email and Password are required.");

        var existingUser = await _userRepo.GetUserByEmail(dto.Email);
        if (existingUser != null)
            throw new ArgumentException("User with this email already exists.");

        var user = new User
        {
            Email = dto.Email,
            Name = dto.Name,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            UserType = dto.UserType,
        };

        var createdUser = await _userRepo.CreateUser(user);
        // If repository returns a User, update the reference to the persisted user
        if (createdUser is null)
            throw new InvalidOperationException("Failed to create user.");
        user = createdUser;

        return GenerateToken(user);
    }

    private AuthResponseDto GenerateToken(User user)
    {
        var jwtSection = _config.GetSection("Jwt");
        var key =
            jwtSection.GetValue<string>("Key")
            ?? throw new InvalidOperationException("JWT Key missing");
        var issuer = jwtSection.GetValue<string>("Issuer") ?? "CSTS";
        var audience = jwtSection.GetValue<string>("Audience") ?? "CSTSUsers";
        var expiresMinutes = jwtSection.GetValue<int>("ExpiresMinutes");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.UserType ?? "customer"),
            new Claim("userId", user.Id.ToString()),
        };

        var keyBytes = Encoding.UTF8.GetBytes(key);
        var creds = new SigningCredentials(
            new SymmetricSecurityKey(keyBytes),
            SecurityAlgorithms.HmacSha256
        );

        var expires = DateTime.UtcNow.AddMinutes(expiresMinutes > 0 ? expiresMinutes : 60);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponseDto
        {
            AccessToken = tokenStr,
            ExpiresAt = expires,
            UserId = user.Id,
            UserType = user.UserType ?? "customer",
            Email = user.Email,
        };
    }
}
