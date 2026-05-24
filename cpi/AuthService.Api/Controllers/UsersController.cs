using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Security;
using AuthService.Api.Contracts;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AuthDbContext _db;

    public UsersController(AuthDbContext db) => _db = db;

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _db.Users
            .Select(u => new UserResponse(u.ID, u.Username, u.Email, u.Role, u.CreatedAt))
            .ToListAsync());

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var u = await _db.Users.FindAsync(id);
        return u is null ? NotFound() : Ok(new UserResponse(u.ID, u.Username, u.Email, u.Role, u.CreatedAt));
    }

    [HttpPut("{id:int}/role")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleRequest req)
    {
        var u = await _db.Users.FindAsync(id);
        if (u is null) return NotFound();
        u.Role = req.Role.Trim();
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:int}/deactivate")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var u = await _db.Users.FindAsync(id);
        if (u is null) return NotFound();
        u.Role = "Deactivated";
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var u = await _db.Users.FindAsync(id);
        if (u is null) return NotFound();
        _db.Users.Remove(u);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:int}/username")]
    [Authorize]
    public async Task<IActionResult> UpdateUsername(int id, [FromBody] UpdateUsernameRequest req)
    {
        if (!IsSelfOrAdmin(id)) return Forbid();

        var username = req.Username.Trim();
        if (string.IsNullOrWhiteSpace(username))
            return BadRequest("Username is required.");

        var u = await _db.Users.FindAsync(id);
        if (u is null) return NotFound();

        if (await _db.Users.AnyAsync(x => x.Username == username && x.ID != id))
            return Conflict("Username already taken.");

        u.Username = username;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:int}/email")]
    [Authorize]
    public async Task<IActionResult> UpdateEmail(int id, [FromBody] UpdateEmailRequest req)
    {
        if (!IsSelfOrAdmin(id)) return Forbid();

        var email = req.Email.Trim();
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email is required.");

        var u = await _db.Users.FindAsync(id);
        if (u is null) return NotFound();

        if (u.PasswordHash is null || u.PasswordSalt is null ||
            !PasswordHasher.Verify(req.Password, u.PasswordHash, u.PasswordSalt))
            return Unauthorized("Wrong password.");

        if (await _db.Users.AnyAsync(x => x.Email == email && x.ID != id))
            return Conflict("Email already taken.");

        u.Email = email;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:int}/password")]
    [Authorize]
    public async Task<IActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordRequest req)
    {
        if (!IsSelfOrAdmin(id)) return Forbid();

        var u = await _db.Users.FindAsync(id);
        if (u is null) return NotFound();

        if (u.PasswordHash is null || u.PasswordSalt is null ||
            !PasswordHasher.Verify(req.CurrentPassword, u.PasswordHash, u.PasswordSalt))
            return Unauthorized("Wrong current password.");

        var (hash, salt) = PasswordHasher.Hash(req.NewPassword);
        u.PasswordHash = hash;
        u.PasswordSalt = salt;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private bool IsSelfOrAdmin(int id)
    {
        var sub  = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
                ?? User.FindFirst("role")?.Value;
        return role == "Admin" || (int.TryParse(sub, out var callerId) && callerId == id);
    }
}
