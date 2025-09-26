using PurchaseOrderService.Application.Purchase;
using PurchaseOrderService.Infrastructure.Purchase;
using PurchaseOrderService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<CpiDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CpiSqlServer")));

// DI
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService.Infrastructure.Purchase.PurchaseOrderService>();
builder.Services.AddScoped<IPurchaseOrderDetailService, PurchaseOrderService.Infrastructure.Purchase.PurchaseOrderDetailService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para tu front
builder.Services.AddCors(o => o.AddPolicy("CPI", p =>
    p.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
     .AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CPI");
app.MapControllers();
app.Run();
