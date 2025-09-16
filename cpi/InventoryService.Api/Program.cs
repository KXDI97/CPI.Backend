using InventoryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// EF Core
builder.Services.AddDbContext<CpiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 👇 ESTA LÍNEA FALTABA
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilita controladores (atribute routing)
app.MapControllers();

// Endpoint de salud opcional
app.MapGet("/health/db", async (CpiDbContext db) =>
{
    var ok = await db.Database.CanConnectAsync();
    return Results.Ok(new { dbCanConnect = ok, utcNow = DateTime.UtcNow });
});

app.Run();
