using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Security;
using AuthService.Domain;
using AuthService.Api.Contracts;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext _db;
    private readonly TokenService _tokens;

    public AuthController(AuthDbContext db, TokenService tokens)
    { _db = db; _tokens = tokens; }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest r)
    {
        if (await _db.Users.AnyAsync(u => u.Username == r.Username || u.Email == r.Email))
            return Conflict("Username o Email ya existen.");

        PasswordHasher.CreateHash(r.Password, out var hash, out var salt);
        var user = new User
        {
            Username = r.Username,
            Email = r.Email,
            Role = r.Role,
            PasswordHash = hash,
            PasswordSalt = salt
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Me), new { }, new { user.ID, user.Username, user.Email, user.Role });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest r)
    {
        var u = await _db.Users.FirstOrDefaultAsync(x =>
            x.Username == r.UsernameOrEmail || x.Email == r.UsernameOrEmail);
        if (u is null || u.PasswordSalt is null || u.PasswordHash is null)
            return Unauthorized("Credenciales inválidas.");

        if (!PasswordHasher.Verify(r.Password, u.PasswordSalt, u.PasswordHash))
            return Unauthorized("Credenciales inválidas.");

        var token = _tokens.Create(u, TimeSpan.FromHours(8));
        return new AuthResponse(token, DateTime.UtcNow.AddHours(8), u.ID, u.Username, u.Email, u.Role);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me() =>
        Ok(new
        {
            Id = User.FindFirst("sub")?.Value,
            Username = User.Identity?.Name ?? User.FindFirst("unique_name")?.Value,
            Email = User.FindFirst("email")?.Value,
            Role = User.FindFirst("role")?.Value
        });

    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        private readonly AuthDbContext _db;
        public HealthController(AuthDbContext db) => _db = db;

        [HttpGet("db")]
        public async Task<IActionResult> Db() =>
            await _db.Database.CanConnectAsync() ? Ok("DB OK") : StatusCode(500, "DB FAIL");
    }

}
