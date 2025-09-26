using Microsoft.AspNetCore.Mvc;
using PurchaseReceiptService.Application.Purchase;

namespace PurchaseReceiptService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseReceiptsController : ControllerBase
{
    private readonly IPurchaseReceiptService _svc;

    public PurchaseReceiptsController(IPurchaseReceiptService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PurchaseReceiptDto>>> GetAll(CancellationToken ct)
        => Ok(await _svc.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PurchaseReceiptDto>> GetById(int id, CancellationToken ct)
    {
        var res = await _svc.GetByIdAsync(id, ct);
        return res is null ? NotFound() : Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseReceiptDto>> Create(CreatePurchaseReceiptDto dto, CancellationToken ct)
    {
        var created = await _svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.ReceiptId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdatePurchaseReceiptDto dto, CancellationToken ct)
    {
        var ok = await _svc.UpdateAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }
}
