// AuthService.Api/Program.cs  (NET 9)
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AuthService.Infrastructure.Security;
using AuthService.Infrastructure.Data;
using AuthService.Api.Contracts; 




var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;

// EF - usa TU contexto
builder.Services.AddDbContext<AuthDbContext>(o =>
    o.UseSqlServer(cfg.GetConnectionString("CpiDb")));

// CORS
builder.Services.AddCors(o =>
    o.AddPolicy("cpi", p => p
        .WithOrigins(cfg.GetSection("Cors:Origins").Get<string[]>()!)
        .AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

// JWT
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = cfg["Jwt:Issuer"],
            ValidAudience = cfg["Jwt:Audience"],
            IssuerSigningKey = key
        };
    });
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger(); app.UseSwaggerUI();

app.UseCors("cpi");
app.UseAuthentication();
app.UseAuthorization();

// ===== Endpoints =====
app.MapPost("/api/auth/register", async (AuthDbContext db, IConfiguration cfg, RegisterRequest req) =>
{
    req = req with { Role = string.IsNullOrWhiteSpace(req.Role) ? "Viewer" : req.Role };
    if (await db.Users.AnyAsync(u => u.Username == req.Username || u.Email == req.Email))
        return Results.Conflict("Usuario o email ya existe");

    var (hash, salt) = PasswordHasher.Hash(req.Password);
    var user = new AuthService.Domain.User
    {
        Username = req.Username.Trim(),
        Email = req.Email.Trim(),
        Role = req.Role.Trim(),
        PasswordHash = hash,
        PasswordSalt = salt
    };
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/api/users/{user.ID}", new UserResponse(user.ID, user.Username, user.Email, user.Role));
});

app.MapPost("/api/auth/login", async (AuthDbContext db, IConfiguration cfg, LoginRequest req) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u =>
        u.Username == req.UsernameOrEmail || u.Email == req.UsernameOrEmail);
    if (user is null || user.PasswordHash is null || user.PasswordSalt is null ||
        !PasswordHasher.Verify(req.Password, user.PasswordHash, user.PasswordSalt))
        return Results.Unauthorized();

    var claims = new[]
    {
        new System.Security.Claims.Claim("sub", user.ID.ToString()),
        new System.Security.Claims.Claim("name", user.Username),
        new System.Security.Claims.Claim("email", user.Email),
        new System.Security.Claims.Claim("role", user.Role)
    };
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        issuer: cfg["Jwt:Issuer"],
        audience: cfg["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(8),
        signingCredentials: creds);
    var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(new AuthResponse(jwt, user.Username, user.Email, user.Role));
});

app.MapGet("/api/users", async (AuthDbContext db) =>
    await db.Users.Select(u => new UserResponse(u.ID, u.Username, u.Email, u.Role)).ToListAsync()
).RequireAuthorization("AdminOnly");

app.MapGet("/api/users/{id:int}", async (int id, AuthDbContext db) =>
{
    var u = await db.Users.FindAsync(id);
    return u is null ? Results.NotFound() : Results.Ok(new UserResponse(u.ID, u.Username, u.Email, u.Role));
}).RequireAuthorization();

app.MapPut("/api/users/{id:int}/role", async (int id, string role, AuthDbContext db) =>
{
    var u = await db.Users.FindAsync(id);
    if (u is null) return Results.NotFound();
    u.Role = role;
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization("AdminOnly");

// (opcional) seed admin
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.Run();
