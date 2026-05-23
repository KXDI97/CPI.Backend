using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AuthService.Infrastructure.Security;
using AuthService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;

builder.Services.AddDbContext<AuthDbContext>(o =>
{
    if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        o.UseNpgsql(cfg.GetConnectionString("PostgreSQL"));
    else
        o.UseSqlServer(cfg.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped<TokenService>();

builder.Services.AddCors(o =>
    o.AddPolicy("cpi", p => p
        .WithOrigins(cfg.GetSection("Cors:Origins").Get<string[]>()!)
        .AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

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
    o.AddPolicy("AdminOnly", p => p.RequireRole("Admin")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("cpi");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
