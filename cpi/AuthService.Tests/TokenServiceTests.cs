using AuthService.Domain;
using AuthService.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthService.Tests;

public class TokenServiceTests
{
    private static TokenService CreateService() =>
        new(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "test-super-secret-key-at-least-32chars!!",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience"
            })
            .Build());

    private static User SampleUser() =>
        new() { ID = 1, Username = "alice", Email = "alice@test.com", Role = "Viewer" };

    [Fact]
    public void Create_ReturnsNonEmptyToken()
    {
        var token = CreateService().Create(SampleUser());

        Assert.False(string.IsNullOrEmpty(token));
    }

    [Fact]
    public void Create_ProducesValidJwt()
    {
        var token = CreateService().Create(SampleUser());

        Assert.True(new JwtSecurityTokenHandler().CanReadToken(token));
    }

    [Fact]
    public void Create_ContainsExpectedClaims()
    {
        var user = SampleUser();
        var token = CreateService().Create(user);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        Assert.Equal("1", jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
        Assert.Equal("alice", jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value);
        Assert.Equal("Viewer", jwt.Claims.First(c => c.Type == ClaimTypes.Role).Value);
    }

    [Fact]
    public void Create_RespectsCustomLifetime()
    {
        var lifetime = TimeSpan.FromMinutes(30);
        var before = DateTime.UtcNow;

        var token = CreateService().Create(SampleUser(), lifetime);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        Assert.True(jwt.ValidTo > before.Add(lifetime - TimeSpan.FromSeconds(5)));
        Assert.True(jwt.ValidTo < before.Add(lifetime + TimeSpan.FromSeconds(5)));
    }
}
