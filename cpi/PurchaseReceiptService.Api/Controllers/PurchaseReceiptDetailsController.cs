using Microsoft.AspNetCore.Mvc;
using PurchaseReceiptService.Application.Purchase;

namespace PurchaseReceiptService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseReceiptDetailsController : ControllerBase
{
    private readonly IPurchaseReceiptDetailService _svc;

    public PurchaseReceiptDetailsController(IPurchaseReceiptDetailService svc) => _svc = svc;

    [HttpGet("receipt/{receiptId:int}")]
    public async Task<ActionResult<IEnumerable<PurchaseReceiptDetailDto>>> GetAllByReceipt(int receiptId, CancellationToken ct)
        => Ok(await _svc.GetAllByReceiptIdAsync(receiptId, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PurchaseReceiptDetailDto>> GetById(int id, CancellationToken ct)
    {
        var res = await _svc.GetByIdAsync(id, ct);
        return res is null ? NotFound() : Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseReceiptDetailDto>> Create(CreatePurchaseReceiptDetailDto dto, CancellationToken ct)
    {
        var created = await _svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.ReceiptDetailId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdatePurchaseReceiptDetailDto dto, CancellationToken ct)
    {
        var ok = await _svc.UpdateAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => (await _svc.DeleteAsync(id, ct)) ? NoContent() : NotFound();
}
