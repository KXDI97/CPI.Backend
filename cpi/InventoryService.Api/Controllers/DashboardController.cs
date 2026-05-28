using InventoryService.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly CpiDbContext _db;

    public DashboardController(CpiDbContext db) => _db = db;

    [HttpGet("kpis")]
    public async Task<IActionResult> GetKpis(CancellationToken ct)
    {
        var totalOrders = await _db.PurchaseOrders.CountAsync(ct);

        var totalSpending = await _db.PurchaseOrderDetails
            .SumAsync(d => (decimal?)d.LineTotal, ct) ?? 0;

        var pendingPayments = await (
            from o in _db.PurchaseOrders
            where o.Status == "Pending"
            join d in _db.PurchaseOrderDetails on o.PurchaseOrderId equals d.PurchaseOrderId
            select (decimal?)d.LineTotal
        ).SumAsync(ct) ?? 0;

        var activeProducts = await _db.Products.CountAsync(p => p.Stock > 0, ct);

        return Ok(new { totalOrders, totalSpending, pendingPayments, activeProducts });
    }

    [HttpGet("recent-storage")]
    public async Task<IActionResult> GetRecentStorage(CancellationToken ct)
    {
        var items = await (
            from p in _db.Products
            join s in _db.Suppliers on p.SupplierId equals s.SupplierId
            orderby p.Stock descending
            select new { p.ProductId, p.Name, p.Category, p.Stock, Supplier = s.Name, p.Value }
        ).Take(10).ToListAsync(ct);

        return Ok(items);
    }

    [HttpGet("purchase-orders-chart")]
    public async Task<IActionResult> GetPurchaseOrdersChart(
        [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, CancellationToken ct)
    {
        var q = _db.PurchaseOrders.AsQueryable();
        if (dateFrom.HasValue) q = q.Where(o => o.OrderDate >= dateFrom.Value);
        if (dateTo.HasValue)   q = q.Where(o => o.OrderDate <= dateTo.Value);

        var raw = await q
            .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync(ct);

        var labels = raw.Select(d => new DateTime(d.Year, d.Month, 1).ToString("MMM yyyy")).ToArray();
        var data   = raw.Select(d => d.Count).ToArray();

        return Ok(new { labels, data });
    }

    [HttpGet("spending-chart")]
    public async Task<IActionResult> GetSpendingChart(
        [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, CancellationToken ct)
    {
        var orders = _db.PurchaseOrders.AsQueryable();
        if (dateFrom.HasValue) orders = orders.Where(o => o.OrderDate >= dateFrom.Value);
        if (dateTo.HasValue)   orders = orders.Where(o => o.OrderDate <= dateTo.Value);

        var filteredIds = orders.Select(o => o.PurchaseOrderId);

        var raw = await (
            from d in _db.PurchaseOrderDetails
            where filteredIds.Contains(d.PurchaseOrderId)
            join p in _db.Products on d.ProductId equals p.ProductId
            group d.LineTotal by p.Category into g
            select new { Category = g.Key, Total = g.Sum() }
        ).ToListAsync(ct);

        return Ok(new
        {
            labels = raw.Select(x => x.Category).ToArray(),
            data   = raw.Select(x => (double)x.Total).ToArray()
        });
    }

    [HttpGet("upcoming-payments")]
    public async Task<IActionResult> GetUpcomingPayments(CancellationToken ct)
    {
        var items = await (
            from s in _db.Sales
            join c in _db.Clients on s.ClientId equals c.ClientId
            where s.Status != "Paid"
            orderby s.InvoiceDate
            select new
            {
                s.InvoiceId,
                ClientName = c.Name,
                s.InvoiceDate,
                s.Total,
                s.Status
            }
        ).Take(6).ToListAsync(ct);

        return Ok(items);
    }
}
