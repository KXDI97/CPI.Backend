using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthService.Infrastructure.Data;
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
}
