using CatalogService.Application.Products;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _svc;

    public ProductsController(IProductService svc) => _svc = svc;

    // GET api/products
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await _svc.GetAllAsync(ct);
        return Ok(items);
    }

    // GET api/products/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var item = await _svc.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // GET api/products/supplier/{supplierId}
    [HttpGet("supplier/{supplierId:int}")]
    public async Task<IActionResult> GetBySupplier(int supplierId, CancellationToken ct)
    {
        var items = await _svc.GetBySupplierAsync(supplierId, ct);
        return Ok(items);
    }

    // POST api/products
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.ProductId }, created);
    }

    // PUT api/products/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProductDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _svc.UpdateAsync(id, dto, ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var deleted = await _svc.DeleteAsync(id, ct);
        return deleted ? NoContent() : NotFound();
    }
}