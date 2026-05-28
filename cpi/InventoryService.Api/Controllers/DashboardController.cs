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
    public async Task<IActionResult> GetKpis(
        [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, CancellationToken ct)
    {
        var orders = _db.PurchaseOrders.AsQueryable();
        if (dateFrom.HasValue) orders = orders.Where(o => o.OrderDate >= dateFrom.Value);
        if (dateTo.HasValue)   orders = orders.Where(o => o.OrderDate < dateTo.Value.AddDays(1));

        var totalOrders = await orders.CountAsync(ct);

        var filteredIds = orders.Select(o => o.PurchaseOrderId);

        var totalSpending = await _db.PurchaseOrderDetails
            .Where(d => filteredIds.Contains(d.PurchaseOrderId))
            .SumAsync(d => (decimal?)d.LineTotal, ct) ?? 0;

        var pendingIds = orders.Where(o => o.Status == "Pending").Select(o => o.PurchaseOrderId);
        var pendingPayments = await _db.PurchaseOrderDetails
            .Where(d => pendingIds.Contains(d.PurchaseOrderId))
            .SumAsync(d => (decimal?)d.LineTotal, ct) ?? 0;

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
        if (dateTo.HasValue)   q = q.Where(o => o.OrderDate < dateTo.Value.AddDays(1));

        var filteredIds = q.Select(o => o.PurchaseOrderId);

        var raw = await (
            from d in _db.PurchaseOrderDetails
            where filteredIds.Contains(d.PurchaseOrderId)
            join o in _db.PurchaseOrders on d.PurchaseOrderId equals o.PurchaseOrderId
            group d.LineTotal by new { o.OrderDate.Year, o.OrderDate.Month } into g
            select new { g.Key.Year, g.Key.Month, Total = g.Sum() }
        ).OrderBy(x => x.Year).ThenBy(x => x.Month).ToListAsync(ct);

        var labels = raw.Select(d => new DateTime(d.Year, d.Month, 1).ToString("MMM yyyy")).ToArray();
        var data   = raw.Select(d => (double)d.Total).ToArray();

        return Ok(new { labels, data });
    }

    [HttpGet("spending-chart")]
    public async Task<IActionResult> GetSpendingChart(
        [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, CancellationToken ct)
    {
        var orders = _db.PurchaseOrders.AsQueryable();
        if (dateFrom.HasValue) orders = orders.Where(o => o.OrderDate >= dateFrom.Value);
        if (dateTo.HasValue)   orders = orders.Where(o => o.OrderDate < dateTo.Value.AddDays(1));

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
    public async Task<IActionResult> GetUpcomingPayments(
        [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, CancellationToken ct)
    {
        var items = await (
            from s in _db.Sales
            join c in _db.Clients on s.ClientId equals c.ClientId
            where s.Status != "Paid"
               && (!dateFrom.HasValue || s.InvoiceDate >= dateFrom.Value)
               && (!dateTo.HasValue   || s.InvoiceDate < dateTo.Value.AddDays(1))
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
