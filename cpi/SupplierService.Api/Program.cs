using Microsoft.EntityFrameworkCore;
using SupplierService.Application.Supplier;
using SupplierService.Infrastructure.Data;
using SupplierService.Infrastructure.Supplier;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CpiDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CpiSqlServer")));
    
builder.Services.AddScoped<ISupplierService, SupplierService.Infrastructure.Supplier.SupplierService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("CPI", p =>
    p.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
     .AllowAnyHeader()
     .AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CPI");
app.MapControllers();

app.Run();
