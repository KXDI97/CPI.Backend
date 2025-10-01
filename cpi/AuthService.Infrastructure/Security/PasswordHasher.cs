// AuthService.Infrastructure/Security/PasswordHasher.cs
using System.Security.Cryptography;

namespace AuthService.Infrastructure.Security;

public static class PasswordHasher
{
    public static (byte[] Hash, byte[] Salt) Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16); // 16 bytes
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        return (pbkdf2.GetBytes(32), salt); // 32 bytes
    }

    public static bool Verify(string password, byte[] hash, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        return CryptographicOperations.FixedTimeEquals(hash, pbkdf2.GetBytes(32));
    }
}
