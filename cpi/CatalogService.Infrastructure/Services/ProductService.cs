using CatalogService.Application.Products;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Services;

public sealed class ProductService : IProductService
{
    private readonly CpiDbContext _db;

    public ProductService(CpiDbContext db) => _db = db;

    // ── GET ALL ─────────────────────────────────────────────────────────────
    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Products
            .AsNoTracking()
            .Select(p => ToDto(p))
            .ToListAsync(ct);
    }

    // ── GET BY SUPPLIER ──────────────────────────────────────────────────────
    public async Task<IEnumerable<ProductDto>> GetBySupplierAsync(int supplierId, CancellationToken ct = default)
    {
        return await _db.Products
            .AsNoTracking()
            .Where(p => p.SupplierId == supplierId)
            .Select(p => ToDto(p))
            .ToListAsync(ct);
    }

    // ── GET BY ID ────────────────────────────────────────────────────────────
    public async Task<ProductDto?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var p = await _db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProductId == id, ct);

        return p is null ? null : ToDto(p);
    }

    // ── CREATE ───────────────────────────────────────────────────────────────
    public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken ct = default)
    {
        var product = new Product
        {
            ProductId   = dto.ProductId,
            Name        = dto.Name,
            Value       = dto.Value,
            Category    = dto.Category,
            Description = dto.Description,
            Stock       = dto.Stock,
            SupplierId  = dto.SupplierId
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);

        return ToDto(product);
    }

    // ── UPDATE ───────────────────────────────────────────────────────────────
    public async Task<bool> UpdateAsync(string id, UpdateProductDto dto, CancellationToken ct = default)
    {
        var product = await _db.Products.FindAsync([id], ct);
        if (product is null) return false;

        product.Name        = dto.Name;
        product.Value       = dto.Value;
        product.Category    = dto.Category;
        product.Description = dto.Description;
        product.Stock       = dto.Stock;
        product.SupplierId  = dto.SupplierId;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    // ── DELETE ───────────────────────────────────────────────────────────────
    public async Task<bool> DeleteAsync(string id, CancellationToken ct = default)
    {
        var product = await _db.Products.FindAsync([id], ct);
        if (product is null) return false;

        _db.Products.Remove(product);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    // ── MAPPER ───────────────────────────────────────────────────────────────
    private static ProductDto ToDto(Product p) => new(
        p.ProductId, p.Name, p.Value,
        p.Category, p.Description, p.Stock, p.SupplierId
    );
}