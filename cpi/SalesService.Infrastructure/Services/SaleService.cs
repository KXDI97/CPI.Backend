using Microsoft.EntityFrameworkCore;
using SalesService.Application.Sales;
using SalesService.Domain.Entities;
using SalesService.Infrastructure.Data;

namespace SalesService.Infrastructure.Services;

public sealed class SaleService : ISaleService
{
    private readonly SalesDbContext _db;

    public SaleService(SalesDbContext db) => _db = db;

    // ── GET ALL (resumen) ────────────────────────────────────────────────────
    public async Task<IEnumerable<SaleSummaryDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Sales
            .AsNoTracking()
            .OrderByDescending(s => s.InvoiceDate)
            .Select(s => new SaleSummaryDto(
                s.InvoiceId,
                s.ClientId,
                "",          // ClientName se resuelve vía CatalogService o join externo
                s.InvoiceDate,
                s.Status,
                s.Total
            ))
            .ToListAsync(ct);
    }

    // ── GET BY ID (con detalle) ──────────────────────────────────────────────
    public async Task<SaleDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var sale = await _db.Sales
            .AsNoTracking()
            .Include(s => s.Details)
            .FirstOrDefaultAsync(s => s.InvoiceId == id, ct);

        return sale is null ? null : ToDto(sale);
    }

    // ── CREATE ───────────────────────────────────────────────────────────────
    public async Task<SaleDto> CreateAsync(CreateSaleDto dto, CancellationToken ct = default)
    {
        if (!dto.Lines.Any())
            throw new ArgumentException("La venta debe tener al menos una línea.");

        var details = dto.Lines.Select(l => new SaleDetail
        {
            ProductId = l.ProductId,
            Quantity  = l.Quantity,
            UnitPrice = l.UnitPrice
        }).ToList();

        // Calcular totales en aplicación (el trigger también actualiza Stock en BD)
        var subtotal = details.Sum(d => d.Quantity * d.UnitPrice);
        var tax      = 0m;   // ajusta si manejas IVA: subtotal * 0.19m
        var total    = subtotal + tax;

        var sale = new Sale
        {
            ClientId     = dto.ClientId,
            InvoiceDate  = dto.InvoiceDate,
            ExchangeRate = dto.ExchangeRate,
            Status       = "Emitida",
            Subtotal     = subtotal,
            Tax          = tax,
            Total        = total,
            Details      = details
        };

        _db.Sales.Add(sale);
        await _db.SaveChangesAsync(ct);

        // Recargar para obtener LineTotal calculado por BD
        await _db.Entry(sale).ReloadAsync(ct);
        foreach (var d in sale.Details)
            await _db.Entry(d).ReloadAsync(ct);

        return ToDto(sale);
    }

    // ── UPDATE STATUS ────────────────────────────────────────────────────────
    public async Task<bool> UpdateStatusAsync(int id, UpdateSaleStatusDto dto, CancellationToken ct = default)
    {
        var sale = await _db.Sales.FindAsync([id], ct);
        if (sale is null) return false;

        sale.Status = dto.Status;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    // ── DELETE ───────────────────────────────────────────────────────────────
    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var sale = await _db.Sales
            .Include(s => s.Details)
            .FirstOrDefaultAsync(s => s.InvoiceId == id, ct);

        if (sale is null) return false;

        _db.Sales.Remove(sale);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    // ── MAPPER ───────────────────────────────────────────────────────────────
    private static SaleDto ToDto(Sale s) => new(
        s.InvoiceId,
        s.ClientId,
        "",
        s.InvoiceDate,
        s.Status,
        s.ExchangeRate,
        s.Subtotal,
        s.Tax,
        s.Total,
        s.Details.Select(d => new SaleDetailDto(
            d.InvoiceDetailId,
            d.ProductId,
            d.Quantity,
            d.UnitPrice,
            d.LineTotal
        ))
    );
}