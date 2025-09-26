using Microsoft.EntityFrameworkCore;
using PurchaseReceiptService.Application.Purchase;
using PurchaseReceiptService.Infrastructure.Data;
using PurchaseReceiptService.Infrastructure.Purchase;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<CpiDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CpiSqlServer")));

builder.Services.AddScoped<IPurchaseReceiptService, PurchaseReceiptService.Infrastructure.Purchase.PurchaseReceiptService>();
builder.Services.AddScoped<IPurchaseReceiptDetailService, PurchaseReceiptService.Infrastructure.Purchase.PurchaseReceiptDetailService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("CPI", p =>
    p.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
     .AllowAnyHeader()
     .AllowAnyMethod()));

var app = builder.Build();

// ✅ Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CPI");
app.MapControllers();

app.Run();
