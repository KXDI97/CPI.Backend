using Microsoft.AspNetCore.Mvc;
using SupplierService.Application.Supplier;

namespace SupplierService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _svc;

    public SuppliersController(ISupplierService svc) => _svc = svc;

    // GET: api/suppliers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAll(CancellationToken ct)
        => Ok(await _svc.GetAllAsync(ct));

    // GET: api/suppliers/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SupplierDto>> GetById(int id, CancellationToken ct)
    {
        var supplier = await _svc.GetByIdAsync(id, ct);
        return supplier is null ? NotFound() : Ok(supplier);
    }

    // POST: api/suppliers
    [HttpPost]
    public async Task<ActionResult<SupplierDto>> Create(CreateSupplierDto dto, CancellationToken ct)
    {
        var created = await _svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.SupplierId }, created);
    }

    // PUT: api/suppliers/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateSupplierDto dto, CancellationToken ct)
    {
        var ok = await _svc.UpdateAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    // DELETE: api/suppliers/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _svc.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
