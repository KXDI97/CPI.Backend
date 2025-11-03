using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CPI.Infrastructure;

// Application Layers
using CPI.Application.Purchase.Dtos;
using CPI.Application.Supplier;
using CPI.Application.Transaction;

// Infrastructure Implementations
using CPI.Infrastructure.Purchase;
using CPI.Infrastructure.Supplier;
using CPI.Infrastructure.Transaction;

// Shared Kernel Interfaces
using CPI.SharedKernel.Interfaces.Purchase;
using CPI.SharedKernel.Interfaces.Supplier;
using CPI.SharedKernel.Interfaces.Transaction;

var builder = WebApplication.CreateBuilder(args);

// =============================================
// 🔹 DATABASE CONFIGURATION
// =============================================
builder.Services.AddDbContext<CpiDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CpiDatabase")));

// =============================================
// 🔹 DEPENDENCY INJECTION (unificados)
// =============================================

// Purchase Order
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IPurchaseOrderDetailService, PurchaseOrderDetailService>();
builder.Services.AddScoped<ILogicalCostService, LogicalCostService>();

// Purchase Receipt
builder.Services.AddScoped<IPurchaseReceiptService, PurchaseReceiptService>();
builder.Services.AddScoped<IPurchaseReceiptDetailService, PurchaseReceiptDetailService>();

// Supplier
builder.Services.AddScoped<ISupplierService, SupplierService>();

// Transaction
builder.Services.AddScoped<ITransactionService, TransactionService>();

// =============================================
// 🔹 CONTROLLERS & SWAGGER
// =============================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CPI Monolith API",
        Version = "v1",
        Description = "Sistema unificado de Compras, Proveedores y Transacciones"
    });
});

// =============================================
// 🔹 CORS CONFIGURATION
// =============================================
builder.Services.AddCors(o => o.AddPolicy("CPI", p =>
    p.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
     .AllowAnyHeader()
     .AllowAnyMethod()));

// =============================================
// 🔹 BUILD APP
// =============================================
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CPI Monolith API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("CPI");
app.UseAuthorization();

app.MapControllers();

app.Run();
