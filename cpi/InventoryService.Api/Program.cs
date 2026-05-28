using InventoryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "CpiCors";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(CorsPolicy, p => p
        .WithOrigins(
            "http://localhost:5500",
            "http://127.0.0.1:5500",
            "http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddDbContext<CpiDbContext>(options =>
{
    if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
    else
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);

// Habilita controladores (atribute routing)
app.MapControllers();

// Endpoint de salud opcional
app.MapGet("/health/db", async (CpiDbContext db) =>
{
    var ok = await db.Database.CanConnectAsync();
    return Results.Ok(new { dbCanConnect = ok, utcNow = DateTime.UtcNow });
});

app.Run();
