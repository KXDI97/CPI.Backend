using CatalogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ConnectionString desde appsettings.json -> "DefaultConnection"
builder.Services.AddDbContext<CpiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// OpenAPI del template
builder.Services.AddOpenApi();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ---- Endpoint de prueba del template (lo puedes dejar) ----
var summaries = new[]
{
    "Freezing","Bracing","Chilly","Cool","Mild","Warm","Balmy","Hot","Sweltering","Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )).ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// ---- Endpoint de salud para probar la DB ----
app.MapGet("/health/db", async (CpiDbContext db) =>
{
    var ok = await db.Database.CanConnectAsync();
    return Results.Ok(new { DbCanConnect = ok, UtcNow = DateTime.UtcNow });
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
