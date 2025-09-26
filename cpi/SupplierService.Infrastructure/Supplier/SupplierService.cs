using Microsoft.EntityFrameworkCore;
using SupplierEntity = SupplierService.Domain.Entities.Supplier;
using SupplierService.Application.Supplier;
using SupplierService.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;


namespace SupplierService.Infrastructure.Supplier;

public class SupplierService : ISupplierService
{
    private readonly CpiDbContext _db;

    public SupplierService(CpiDbContext db) => _db = db;

    public async Task<IEnumerable<SupplierDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Suppliers
            .Select(s => new SupplierDto(
                s.SupplierId,
                s.Name,
                s.Contact,
                s.Phone,
                s.Email,
                s.Address
            ))
            .ToListAsync(ct);
    }

    public async Task<SupplierDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var s = await _db.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id, ct);
        return s is null ? null :
            new SupplierDto(s.SupplierId, s.Name, s.Contact, s.Phone, s.Email, s.Address);
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto, CancellationToken ct = default)
    {
         var entity = new SupplierEntity
        {
            Name = dto.Name,
            Contact = dto.Contact,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address
        };

        _db.Suppliers.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new SupplierDto(entity.SupplierId, entity.Name, entity.Contact, entity.Phone, entity.Email, entity.Address);
    }

    public async Task<bool> UpdateAsync(int id, UpdateSupplierDto dto, CancellationToken ct = default)
    {
        var entity = await _db.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id, ct);
        if (entity is null) return false;

        entity.Name = dto.Name;
        entity.Contact = dto.Contact;
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.Address = dto.Address;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id, ct);
        if (entity is null) return false;

        _db.Suppliers.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
