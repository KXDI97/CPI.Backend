using CatalogService.Application.Clients;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Clients;


public class ClientService : IClientService
{
    private readonly CpiDbContext _db;
    public ClientService(CpiDbContext db) => _db = db;

    public async Task<IEnumerable<ClientDto>> GetAllAsync(CancellationToken ct = default)
        => await _db.Clients.OrderBy(c => c.Name)
            .Select(c => new ClientDto(c.ClientId, c.Name, c.ClientType, c.DocumentType, c.DocumentID, c.Email, c.Phone, c.Website, c.Address))
            .ToListAsync(ct);

    public async Task<ClientDto?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Clients.Where(c => c.ClientId == id)
            .Select(c => new ClientDto(c.ClientId, c.Name, c.ClientType, c.DocumentType, c.DocumentID, c.Email, c.Phone, c.Website, c.Address))
            .FirstOrDefaultAsync(ct);

    public async Task<ClientDto> CreateAsync(CreateClientDto dto, CancellationToken ct = default)
    {
        bool exists = await _db.Clients.AnyAsync(c =>
            c.DocumentType == dto.DocumentType && c.DocumentID == dto.DocumentID, ct);
        if (exists) throw new InvalidOperationException("Cliente ya existe (tipo + documento).");

        var entity = new Client
        {
            Name = dto.Name.Trim(),
            ClientType = dto.ClientType,
            DocumentType = dto.DocumentType,
            DocumentID = dto.DocumentID.Trim(),
            Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email,
            Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone,
            Website = string.IsNullOrWhiteSpace(dto.Website) ? null : dto.Website,
            Address = string.IsNullOrWhiteSpace(dto.Address) ? null : dto.Address
        };

        _db.Clients.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new ClientDto(entity.ClientId, entity.Name, entity.ClientType, entity.DocumentType, entity.DocumentID, entity.Email, entity.Phone, entity.Website, entity.Address);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientDto dto, CancellationToken ct = default)
    {
        var c = await _db.Clients.FindAsync(new object?[] { id }, ct);
        if (c is null) return false;

        if (c.DocumentType != dto.DocumentType || c.DocumentID != dto.DocumentID)
        {
            bool exists = await _db.Clients.AnyAsync(x =>
                x.ClientId != id &&
                x.DocumentType == dto.DocumentType &&
                x.DocumentID == dto.DocumentID, ct);
            if (exists) throw new InvalidOperationException("Otro cliente ya tiene ese tipo+documento.");
        }

        c.Name = dto.Name.Trim();
        c.ClientType = dto.ClientType;
        c.DocumentType = dto.DocumentType;
        c.DocumentID = dto.DocumentID.Trim();
        c.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email;
        c.Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone;
        c.Website = string.IsNullOrWhiteSpace(dto.Website) ? null : dto.Website;
        c.Address = string.IsNullOrWhiteSpace(dto.Address) ? null : dto.Address;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var c = await _db.Clients.FindAsync(new object?[] { id }, ct);
        if (c is null) return false;
        _db.Clients.Remove(c); // si quieres soft delete, luego añadimos IsActive
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
