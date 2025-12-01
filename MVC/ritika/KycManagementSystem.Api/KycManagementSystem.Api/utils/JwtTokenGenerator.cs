using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KycManagementSystem.Api.Models.Entities;
using Microsoft.IdentityModel.Tokens;
namespace KycManagementSystem.Api.utils
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
    public class JwtTokenGenerator: IJwtTokenGenerator
    {
        private readonly string _secret;
        private readonly int _expiryMinutes;

        public JwtTokenGenerator(string secret, int expiryMinutes= 30)
        {
            _secret = secret;
            _expiryMinutes = expiryMinutes;
        }
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
