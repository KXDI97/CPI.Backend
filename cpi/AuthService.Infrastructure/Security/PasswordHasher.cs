using System.Security.Cryptography;

namespace AuthService.Infrastructure.Security;

public static class PasswordHasher
{
    public static void CreateHash(string password, out byte[] hash, out byte[] salt, int iterations = 120_000)
    {
        salt = RandomNumberGenerator.GetBytes(16);

        hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            32); // 32 bytes = 256 bits
    }

    public static bool Verify(string password, byte[] salt, byte[] hash, int iterations = 120_000)
    {
        var testHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            32);

        return CryptographicOperations.FixedTimeEquals(testHash, hash);
    }
}
