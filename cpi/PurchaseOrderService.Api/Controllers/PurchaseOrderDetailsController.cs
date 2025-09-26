using Microsoft.AspNetCore.Mvc;
using PurchaseOrderService.Application.Purchase;

namespace PurchaseOrderService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrderDetailsController : ControllerBase
{
    private readonly IPurchaseOrderDetailService _svc;

    public PurchaseOrderDetailsController(IPurchaseOrderDetailService svc) => _svc = svc;

    [HttpGet("order/{orderId:int}")]
    public async Task<ActionResult<IEnumerable<PurchaseOrderDetailDto>>> GetAllByOrder(int orderId, CancellationToken ct)
        => Ok(await _svc.GetAllByOrderIdAsync(orderId, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PurchaseOrderDetailDto>> GetById(int id, CancellationToken ct)
    {
        var res = await _svc.GetByIdAsync(id, ct);
        return res is null ? NotFound() : Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseOrderDetailDto>> Create(CreatePurchaseOrderDetailDto dto, CancellationToken ct)
    {
        var created = await _svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.PurchaseOrderDetailId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdatePurchaseOrderDetailDto dto, CancellationToken ct)
    {
        var ok = await _svc.UpdateAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => (await _svc.DeleteAsync(id, ct)) ? NoContent() : NotFound();
}
