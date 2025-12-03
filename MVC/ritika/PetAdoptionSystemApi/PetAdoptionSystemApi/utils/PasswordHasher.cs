using System;
using System.Security.Cryptography;
using System.Text;

namespace PetAdoptionSystemApi.utils
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool Verify(string password, string storedHash)
        {
            return Hash(password) == storedHash;
        }
    }
}
