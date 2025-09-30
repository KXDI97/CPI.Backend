using CatalogService.Api.Clients;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogService.Api.Clients;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly CpiDbContext _db;
    public ClientsController(CpiDbContext db) => _db = db;

    // GET /api/clients?page=1&pageSize=12&q=texto&sort=name&dir=asc
    [HttpGet]
    public async Task<ActionResult<PagedResult<ClientDto>>> Get(
        int page = 1,
        int pageSize = 12,
        string? q = null,
        string sort = "name",
        string dir = "asc")
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0 || pageSize > 200) pageSize = 12;

        var query = _db.Clients.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var t = q.Trim().ToLower();
            query = query.Where(c =>
                EF.Functions.Like(c.Name.ToLower(), $"%{t}%") ||
                EF.Functions.Like(c.DocumentID.ToLower(), $"%{t}%") ||
                EF.Functions.Like(c.DocumentType.ToLower(), $"%{t}%") ||
                (c.Email != null && EF.Functions.Like(c.Email.ToLower(), $"%{t}%")) ||
                (c.Phone != null && EF.Functions.Like(c.Phone.ToLower(), $"%{t}%"))
            );
        }

        // Ordenamiento simple
        Expression<Func<Client, object>> keySelector = sort.ToLower() switch
        {
            "documentid"   => c => c.DocumentID,
            "documenttype" => c => c.DocumentType,
            "clienttype"   => c => c.ClientType,
            "email"        => c => c.Email ?? "",
            "name"         => c => c.Name,
            _              => c => c.Name
        };

        query = (dir.ToLower() == "desc")
            ? query.OrderByDescending(keySelector)
            : query.OrderBy(keySelector);

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ClientDto(
                c.ClientId, c.Name, c.ClientType, c.DocumentType, c.DocumentID,
                c.Email, c.Phone, c.Website, c.Address))
            .ToListAsync();

        return Ok(new PagedResult<ClientDto>(items, total));
    }

    // GET /api/clients/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        var c = await _db.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == id);
        if (c == null) return NotFound();
        return new ClientDto(c.ClientId, c.Name, c.ClientType, c.DocumentType, c.DocumentID, c.Email, c.Phone, c.Website, c.Address);
    }

    // POST /api/clients
    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create([FromBody] ClientCreateDto dto)
    {
        var entity = new Client
        {
            Name = dto.Name.Trim(),
            ClientType = dto.ClientType,
            DocumentType = dto.DocumentType,
            DocumentID = dto.DocumentID.Trim(),
            Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim(),
            Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone.Trim(),
            Website = string.IsNullOrWhiteSpace(dto.Website) ? null : dto.Website.Trim(),
            Address = string.IsNullOrWhiteSpace(dto.Address) ? null : dto.Address.Trim()
        };

        _db.Clients.Add(entity);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            return Conflict("Ya existe un cliente con ese tipo y número de documento.");
        }

        var result = new ClientDto(entity.ClientId, entity.Name, entity.ClientType, entity.DocumentType, entity.DocumentID, entity.Email, entity.Phone, entity.Website, entity.Address);

        return CreatedAtAction(nameof(GetById), new { id = entity.ClientId }, result);
    }

    // PUT /api/clients/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ClientDto>> Update(int id, [FromBody] ClientUpdateDto dto)
    {
        var entity = await _db.Clients.FirstOrDefaultAsync(x => x.ClientId == id);
        if (entity == null) return NotFound();

        entity.Name = dto.Name.Trim();
        entity.ClientType = dto.ClientType;
        entity.DocumentType = dto.DocumentType;
        entity.DocumentID = dto.DocumentID.Trim();
        entity.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
        entity.Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone.Trim();
        entity.Website = string.IsNullOrWhiteSpace(dto.Website) ? null : dto.Website.Trim();
        entity.Address = string.IsNullOrWhiteSpace(dto.Address) ? null : dto.Address.Trim();

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            return Conflict("Conflicto de documento (tipo + número ya existe).");
        }

        return new ClientDto(entity.ClientId, entity.Name, entity.ClientType, entity.DocumentType,
                             entity.DocumentID, entity.Email, entity.Phone, entity.Website, entity.Address);
    }

    // DELETE /api/clients/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Clients.FirstOrDefaultAsync(x => x.ClientId == id);
        if (entity == null) return NotFound();

        _db.Clients.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // SQL Server: 2601/2627 claves únicas
    private static bool IsUniqueViolation(DbUpdateException ex)
    {
        if (ex.InnerException is SqlException sqlEx)
            return sqlEx.Number is 2601 or 2627;
        return false;
    }
}
