using AuthService.Api.Contracts;
using AuthService.Api.Controllers;
using AuthService.Domain;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthService.Tests;

public class AuthControllerTests
{
    private static AuthDbContext CreateDb() =>
        new(new DbContextOptionsBuilder<AuthDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static TokenService CreateTokenService() =>
        new(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "test-super-secret-key-at-least-32chars!!",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience"
            })
            .Build());

    private static AuthController CreateController(AuthDbContext db) =>
        new(db, CreateTokenService());

    private static async Task SeedUser(AuthDbContext db,
        string username = "alice", string email = "alice@test.com", string password = "Pass123!")
    {
        var (hash, salt) = PasswordHasher.Hash(password);
        db.Users.Add(new User { Username = username, Email = email, Role = "Viewer", PasswordHash = hash, PasswordSalt = salt });
        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task Register_ReturnsCreated_ForNewUser()
    {
        using var db = CreateDb();
        var result = await CreateController(db).Register(
            new RegisterRequest("alice", "alice@test.com", "Pass123!", "Viewer"));

        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task Register_PersistsUser_InDatabase()
    {
        using var db = CreateDb();
        await CreateController(db).Register(
            new RegisterRequest("bob", "bob@test.com", "Pass123!", "Admin"));

        Assert.True(await db.Users.AnyAsync(u => u.Username == "bob"));
    }

    [Fact]
    public async Task Register_ReturnsConflict_WhenDuplicateUsername()
    {
        using var db = CreateDb();
        await SeedUser(db);

        var result = await CreateController(db).Register(
            new RegisterRequest("alice", "other@test.com", "Pass123!", "Viewer"));

        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public async Task Register_ReturnsConflict_WhenDuplicateEmail()
    {
        using var db = CreateDb();
        await SeedUser(db);

        var result = await CreateController(db).Register(
            new RegisterRequest("other", "alice@test.com", "Pass123!", "Viewer"));

        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsToken_WhenValidCredentials()
    {
        using var db = CreateDb();
        await SeedUser(db);

        var result = await CreateController(db).Login(new LoginRequest("alice", "Pass123!"));

        Assert.NotNull(result.Value);
        Assert.False(string.IsNullOrEmpty(result.Value!.Token));
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenWrongPassword()
    {
        using var db = CreateDb();
        await SeedUser(db);

        var result = await CreateController(db).Login(new LoginRequest("alice", "WrongPass!"));

        Assert.IsType<UnauthorizedObjectResult>(result.Result);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
    {
        using var db = CreateDb();

        var result = await CreateController(db).Login(new LoginRequest("nobody", "pass"));

        Assert.IsType<UnauthorizedObjectResult>(result.Result);
    }

    [Fact]
    public async Task Login_AcceptsEmail_AsUsernameOrEmail()
    {
        using var db = CreateDb();
        await SeedUser(db);

        var result = await CreateController(db).Login(new LoginRequest("alice@test.com", "Pass123!"));

        Assert.NotNull(result.Value);
        Assert.False(string.IsNullOrEmpty(result.Value!.Token));
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenAccountDeactivated()
    {
        using var db = CreateDb();
        var (hash, salt) = PasswordHasher.Hash("Pass123!");
        db.Users.Add(new User { Username = "disabled", Email = "disabled@test.com", Role = "Deactivated", PasswordHash = hash, PasswordSalt = salt });
        await db.SaveChangesAsync();

        var result = await CreateController(db).Login(new LoginRequest("disabled", "Pass123!"));

        var unauth = Assert.IsType<UnauthorizedObjectResult>(result.Result);
        Assert.Equal("ACCOUNT_DEACTIVATED", unauth.Value);
    }

    [Fact]
    public async Task Refresh_ReturnsUnauthorized_WhenAccountDeactivated()
    {
        using var db = CreateDb();
        var (hash, salt) = PasswordHasher.Hash("Pass123!");
        var user = new User { Username = "disabled", Email = "disabled@test.com", Role = "Viewer", PasswordHash = hash, PasswordSalt = salt };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        // Login while still active to get a refresh token
        var ctrl     = CreateController(db);
        var loginRes = await ctrl.Login(new LoginRequest("disabled", "Pass123!"));
        var rt       = loginRes.Value!.RefreshToken;

        // Now deactivate the user
        user.Role = "Deactivated";
        await db.SaveChangesAsync();

        var result = await ctrl.Refresh(new RefreshRequest(rt));

        var unauth = Assert.IsType<UnauthorizedObjectResult>(result.Result);
        Assert.Equal("ACCOUNT_DEACTIVATED", unauth.Value);
    }
}
