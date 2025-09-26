using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TransactionService.Application.Transaction;
using TransactionService.Infrastructure.Data;
using TransactionService.Infrastructure.Transaction;

var builder = WebApplication.CreateBuilder(args);

// 🔹 DbContext
builder.Services.AddDbContext<CpiDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CpiSqlServer")));

// 🔹 Servicios
builder.Services.AddScoped<ITransactionService, TransactionService.Infrastructure.Transaction.TransactionService>();

// 🔹 Controllers y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Transaction API",
        Version = "v1"
    });
});

// 🔹 CORS
builder.Services.AddCors(o => o.AddPolicy("CPI", p =>
    p.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
     .AllowAnyHeader()
     .AllowAnyMethod()));

var app = builder.Build();

// 🔹 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction API V1");
    });
}

app.UseHttpsRedirection();
app.UseCors("CPI");
app.UseAuthorization();

app.MapControllers();

app.Run();
