using CatalogService.Application.Suppliers;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _svc;

    public SuppliersController(ISupplierService svc) => _svc = svc;

    // GET api/suppliers
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await _svc.GetAllAsync(ct);
        return Ok(items);
    }

    // GET api/suppliers/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var item = await _svc.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // POST api/suppliers
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSupplierDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.SupplierId }, created);
    }

    // PUT api/suppliers/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _svc.UpdateAsync(id, dto, ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE api/suppliers/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _svc.DeleteAsync(id, ct);
        return deleted ? NoContent() : NotFound();
    }
}