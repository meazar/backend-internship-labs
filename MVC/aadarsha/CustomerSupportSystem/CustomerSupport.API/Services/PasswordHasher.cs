using System.Security.Cryptography;

namespace CustomerSupport.API.Services;

public static class PasswordHasher
{
    // Format: {iterations}.{saltBase64}.{hashBase64}
    public static string Hash(string password, int iterations = 100_000)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[16];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256
        );
        var hash = pbkdf2.GetBytes(32);

        return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string hashed)
    {
        if (string.IsNullOrWhiteSpace(hashed))
            return false;
        var parts = hashed.Split('.');
        if (parts.Length != 3)
            return false;

        if (!int.TryParse(parts[0], out var iterations))
            return false;
        var salt = Convert.FromBase64String(parts[1]);
        var storedHash = Convert.FromBase64String(parts[2]);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256
        );
        var candidate = pbkdf2.GetBytes(storedHash.Length);

        return CryptographicOperations.FixedTimeEquals(candidate, storedHash);
    }
}
