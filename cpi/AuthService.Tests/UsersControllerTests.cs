using AuthService.Api.Contracts;
using AuthService.Api.Controllers;
using AuthService.Domain;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Tests;

public class UsersControllerTests
{
    private static AuthDbContext CreateDb() =>
        new(new DbContextOptionsBuilder<AuthDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static async Task<User> AddUser(AuthDbContext db,
        string username = "alice", string email = "alice@test.com", string role = "Viewer")
    {
        var (hash, salt) = PasswordHasher.Hash("Pass123!");
        var user = new User { Username = username, Email = email, Role = role, PasswordHash = hash, PasswordSalt = salt };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task GetAll_ReturnsAllUsers()
    {
        using var db = CreateDb();
        await AddUser(db, "alice", "alice@test.com");
        await AddUser(db, "bob", "bob@test.com");

        var result = await new UsersController(db).GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        var users = Assert.IsAssignableFrom<IEnumerable<UserResponse>>(ok.Value);
        Assert.Equal(2, users.Count());
    }

    [Fact]
    public async Task GetById_ReturnsUser_WhenExists()
    {
        using var db = CreateDb();
        var user = await AddUser(db);

        var result = await new UsersController(db).GetById(user.ID);

        var ok = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<UserResponse>(ok.Value);
        Assert.Equal("alice", dto.Username);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        using var db = CreateDb();

        var result = await new UsersController(db).GetById(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateRole_ReturnsNoContent_AndChangesRole()
    {
        using var db = CreateDb();
        var user = await AddUser(db, role: "Viewer");

        var result = await new UsersController(db).UpdateRole(user.ID, new UpdateRoleRequest("Admin"));

        Assert.IsType<NoContentResult>(result);
        var updated = await db.Users.FindAsync(user.ID);
        Assert.Equal("Admin", updated!.Role);
    }

    [Fact]
    public async Task UpdateRole_ReturnsNotFound_WhenMissing()
    {
        using var db = CreateDb();

        var result = await new UsersController(db).UpdateRole(999, new UpdateRoleRequest("Admin"));

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_AndRemovesUser()
    {
        using var db = CreateDb();
        var user = await AddUser(db);

        var result = await new UsersController(db).Delete(user.ID);

        Assert.IsType<NoContentResult>(result);
        Assert.False(await db.Users.AnyAsync(u => u.ID == user.ID));
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenMissing()
    {
        using var db = CreateDb();

        var result = await new UsersController(db).Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Deactivate_ReturnsNoContent_AndSetsRoleDeactivated()
    {
        using var db = CreateDb();
        var user = await AddUser(db, role: "Seller");

        var result = await new UsersController(db).Deactivate(user.ID);

        Assert.IsType<NoContentResult>(result);
        var updated = await db.Users.FindAsync(user.ID);
        Assert.Equal("Deactivated", updated!.Role);
    }

    [Fact]
    public async Task Deactivate_ReturnsNotFound_WhenMissing()
    {
        using var db = CreateDb();

        var result = await new UsersController(db).Deactivate(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
