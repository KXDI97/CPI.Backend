using CatalogService.Application.Suppliers;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Services;

public sealed class SupplierService : ISupplierService
{
    private readonly CpiDbContext _db;

    public SupplierService(CpiDbContext db) => _db = db;

    public async Task<IEnumerable<SupplierDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Suppliers
            .AsNoTracking()
            .Select(s => ToDto(s))
            .ToListAsync(ct);
    }

    public async Task<SupplierDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var s = await _db.Suppliers.AsNoTracking()
            .FirstOrDefaultAsync(x => x.SupplierId == id, ct);
        return s is null ? null : ToDto(s);
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto, CancellationToken ct = default)
    {
        var supplier = new Supplier
        {
            Name    = dto.Name,
            Contact = dto.Contact,
            Phone   = dto.Phone,
            Email   = dto.Email,
            Address = dto.Address
        };
        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync(ct);
        return ToDto(supplier);
    }

    public async Task<bool> UpdateAsync(int id, UpdateSupplierDto dto, CancellationToken ct = default)
    {
        var supplier = await _db.Suppliers.FindAsync([id], ct);
        if (supplier is null) return false;
        supplier.Name    = dto.Name;
        supplier.Contact = dto.Contact;
        supplier.Phone   = dto.Phone;
        supplier.Email   = dto.Email;
        supplier.Address = dto.Address;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var supplier = await _db.Suppliers.FindAsync([id], ct);
        if (supplier is null) return false;
        _db.Suppliers.Remove(supplier);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    private static SupplierDto ToDto(Supplier s) => new(
        s.SupplierId, s.Name, s.Contact, s.Phone, s.Email, s.Address
    );
}