using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Security;
using AuthService.Domain;
using AuthService.Api.Contracts;
using System.Security.Claims;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext _db;
    private readonly TokenService _tokens;

    public AuthController(AuthDbContext db, TokenService tokens)
    {
        _db = db;
        _tokens = tokens;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest r)
    {
        var role = string.IsNullOrWhiteSpace(r.Role) ? "Viewer" : r.Role.Trim();

        if (await _db.Users.AnyAsync(u => u.Username == r.Username || u.Email == r.Email))
            return Conflict("Username o Email ya existen.");

var (hash, salt) = PasswordHasher.Hash(r.Password);


        var user = new User
        {
            Username = r.Username.Trim(),
            Email = r.Email.Trim(),
            Role = role,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Created($"/api/users/{user.ID}", new UserResponse(user.ID, user.Username, user.Email, user.Role, user.CreatedAt));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest r)
    {
        var u = await _db.Users.FirstOrDefaultAsync(x =>
            x.Username == r.UsernameOrEmail || x.Email == r.UsernameOrEmail);

        if (u is null || u.PasswordHash is null || u.PasswordSalt is null)
            return Unauthorized("Credenciales inválidas.");

        if (!PasswordHasher.Verify(r.Password, u.PasswordHash, u.PasswordSalt))
            return Unauthorized("Credenciales inválidas.");

        var token        = _tokens.Create(u, TimeSpan.FromMinutes(15));
        var refreshToken = _tokens.GenerateRefreshToken();

        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId    = u.ID,
            Token     = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            IsRevoked = false
        });
        await _db.SaveChangesAsync();

        return new AuthResponse(token, refreshToken, u.Username, u.Email, u.Role);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshRequest r)
    {
        var rt = await _db.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == r.RefreshToken && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow);

        if (rt is null) return Unauthorized("Refresh token inválido o expirado.");

        var user = await _db.Users.FindAsync(rt.UserId);
        if (user is null) return Unauthorized();

        rt.IsRevoked = true;

        var newToken        = _tokens.Create(user, TimeSpan.FromMinutes(15));
        var newRefreshToken = _tokens.GenerateRefreshToken();

        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId    = user.ID,
            Token     = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            IsRevoked = false
        });
        await _db.SaveChangesAsync();

        return new AuthResponse(newToken, newRefreshToken, user.Username, user.Email, user.Role);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshRequest r)
    {
        var rt = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == r.RefreshToken);
        if (rt is not null) rt.IsRevoked = true;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            Id = User.FindFirst("sub")?.Value,
            Username = User.Identity?.Name,
            Email = User.FindFirst("email")?.Value,
            Role = User.FindFirst("role")?.Value
        });
    }
}
