using System.Security.Cryptography;
using System.Text;

namespace APIBusSeatBookingManagement.Service
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword (string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);

        }

        public bool VerifyPassword(string password,string hash)
        {
            var hashedPasswords = HashPassword(password);
            return hashedPasswords == hash;
        }
    }
}
