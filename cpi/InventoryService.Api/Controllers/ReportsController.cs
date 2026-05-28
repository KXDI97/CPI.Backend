using InventoryService.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Api.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly CpiDbContext _db;

    public ReportsController(CpiDbContext db) => _db = db;

    [HttpGet("sales")]
    public async Task<IActionResult> GetSales(
        [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, CancellationToken ct)
    {
        var rows = await (
            from s in _db.Sales
            join c in _db.Clients on s.ClientId equals c.ClientId
            where (!dateFrom.HasValue || s.InvoiceDate >= dateFrom.Value)
               && (!dateTo.HasValue   || s.InvoiceDate <= dateTo.Value)
            orderby s.InvoiceDate descending
            select new { s.InvoiceId, date = s.InvoiceDate, client = c.Name, s.Total, s.Status }
        ).ToListAsync(ct);

        var invoiceIds = rows.Select(r => r.InvoiceId).ToList();
        var itemCounts = await _db.SalesDetails
            .Where(d => invoiceIds.Contains(d.InvoiceId))
            .GroupBy(d => d.InvoiceId)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.Count, ct);

        return Ok(rows.Select(r => new
        {
            date      = r.date.ToString("yyyy-MM-dd"),
            r.client,
            itemCount = itemCounts.TryGetValue(r.InvoiceId, out var n) ? n : 0,
            total     = (double)r.Total,
            status    = NormaliseStatus(r.Status)
        }));
    }

    [HttpGet("purchases")]
    public async Task<IActionResult> GetPurchases(
        [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, CancellationToken ct)
    {
        var orders = await (
            from o in _db.PurchaseOrders
            join s in _db.Suppliers on o.SupplierId equals s.SupplierId
            where (!dateFrom.HasValue || o.OrderDate >= dateFrom.Value)
               && (!dateTo.HasValue   || o.OrderDate <= dateTo.Value)
            orderby o.OrderDate descending
            select new { o.PurchaseOrderId, date = o.OrderDate, supplier = s.Name, o.Status }
        ).ToListAsync(ct);

        var orderIds = orders.Select(o => o.PurchaseOrderId).ToList();
        var detailMap = await _db.PurchaseOrderDetails
            .Where(d => orderIds.Contains(d.PurchaseOrderId))
            .GroupBy(d => d.PurchaseOrderId)
            .Select(g => new { g.Key, Total = (double)g.Sum(d => d.LineTotal), Count = g.Count() })
            .ToDictionaryAsync(x => x.Key, ct);

        return Ok(orders.Select(o =>
        {
            detailMap.TryGetValue(o.PurchaseOrderId, out var t);
            return new
            {
                date      = o.date.ToString("yyyy-MM-dd"),
                o.supplier,
                itemCount = t?.Count ?? 0,
                total     = t?.Total ?? 0.0,
                status    = NormaliseStatus(o.Status)
            };
        }));
    }

    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventory(
        [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, CancellationToken ct)
    {
        var movQ = _db.InventoryMovements.AsQueryable();
        if (dateFrom.HasValue) movQ = movQ.Where(m => m.CreatedAt >= dateFrom.Value);
        if (dateTo.HasValue)   movQ = movQ.Where(m => m.CreatedAt <= dateTo.Value);

        var movements = await movQ
            .GroupBy(m => m.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                Entries   = (int)g.Where(m => m.QtyChange > 0).Sum(m => (double)m.QtyChange),
                Exits     = (int)g.Where(m => m.QtyChange < 0).Sum(m => -(double)m.QtyChange),
            })
            .ToListAsync(ct);

        var productIds = movements.Select(m => m.ProductId).ToList();
        var products = await _db.Products
            .Where(p => productIds.Contains(p.ProductId))
            .Select(p => new { p.ProductId, p.Name, p.Category, p.Stock })
            .ToDictionaryAsync(p => p.ProductId, ct);

        return Ok(movements
            .Select(m =>
            {
                products.TryGetValue(m.ProductId, out var p);
                return new
                {
                    product  = p?.Name ?? m.ProductId,
                    category = p?.Category ?? "—",
                    entries  = m.Entries,
                    exits    = m.Exits,
                    stock    = p?.Stock ?? 0
                };
            })
            .OrderBy(x => x.product));
    }

    private static string NormaliseStatus(string s) => s?.ToLower() switch
    {
        "paid"      => "paid",
        "emitida"   => "issued",
        "issued"    => "issued",
        "pending"   => "pending",
        "overdue"   => "overdue",
        "received"  => "received",
        "cancelled" => "cancelled",
        _           => (s ?? "pending").ToLower()
    };
}
