using AuthService.Infrastructure.Security;

namespace AuthService.Tests;

public class PasswordHasherTests
{
    [Fact]
    public void Hash_ReturnsNonEmptyHashAndSalt()
    {
        var (hash, salt) = PasswordHasher.Hash("mypassword");

        Assert.NotEmpty(hash);
        Assert.NotEmpty(salt);
    }

    [Fact]
    public void Verify_ReturnsTrue_WhenPasswordMatches()
    {
        var (hash, salt) = PasswordHasher.Hash("mypassword");

        Assert.True(PasswordHasher.Verify("mypassword", hash, salt));
    }

    [Fact]
    public void Verify_ReturnsFalse_WhenPasswordWrong()
    {
        var (hash, salt) = PasswordHasher.Hash("mypassword");

        Assert.False(PasswordHasher.Verify("wrongpassword", hash, salt));
    }

    [Fact]
    public void Hash_GeneratesDifferentSalts_ForSamePassword()
    {
        var (_, salt1) = PasswordHasher.Hash("samepassword");
        var (_, salt2) = PasswordHasher.Hash("samepassword");

        Assert.False(salt1.SequenceEqual(salt2));
    }
}
