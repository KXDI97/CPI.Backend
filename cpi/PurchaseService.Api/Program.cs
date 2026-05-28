using Microsoft.EntityFrameworkCore;
using PurchaseService.Application.PurchaseOrders;
using PurchaseService.Application.PurchaseReceipts;
using PurchaseService.Application.Transactions;
using PurchaseService.Application.LogicalCosts;
using PurchaseService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// CORS
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CpiDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPurchaseOrderService,   PurchaseOrderService>();
builder.Services.AddScoped<IPurchaseReceiptService, PurchaseReceiptService>();
builder.Services.AddScoped<ITransactionService,     TransactionService>();
builder.Services.AddScoped<ILogicalCostService,     LogicalCostService>();
builder.Services.AddScoped<IPurchaseOrderDetailService, PurchaseOrderDetailService>();
builder.Services.AddScoped<IPurchaseReceiptDetailService, PurchaseReceiptDetailService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(CorsPolicy);      // ← CORS antes de controllers
app.UseAuthorization();
app.MapControllers();
app.Run();