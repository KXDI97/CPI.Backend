using Microsoft.EntityFrameworkCore;
using PurchaseService.Domain.Entities;
using PurchaseService.Application.PurchaseOrders.Dtos;
using PurchaseService.Infrastructure.Data;
using PurchaseService.Application.PurchaseOrders;

namespace PurchaseService.Application.PurchaseOrders;
public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly CpiDbContext _context;

    public PurchaseOrderService(CpiDbContext context) => _context = context;

    public async Task<IEnumerable<PurchaseOrderDto>> GetAllAsync(CancellationToken ct = default)
        => await _context.PurchaseOrders
            .Select(po => new PurchaseOrderDto(
                po.PurchaseOrderId, po.OrderDate, po.SupplierId, po.Status
            ))
            .ToListAsync(ct);

    public async Task<PurchaseOrderDto?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.PurchaseOrders
            .Where(po => po.PurchaseOrderId == id)
            .Select(po => new PurchaseOrderDto(
                po.PurchaseOrderId, po.OrderDate, po.SupplierId, po.Status
            ))
            .FirstOrDefaultAsync(ct);

  public async Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto dto, CancellationToken ct = default)
{
    var entity = new PurchaseOrder
    {
        SupplierId = dto.SupplierId,
        OrderDate  = dto.OrderDate == default ? DateTime.UtcNow : dto.OrderDate,
        Status     = "Pendiente",
        Details    = dto.Details.Select(d => new PurchaseOrderDetail
        {
            ProductId = d.ProductId,
            Quantity  = d.Quantity,
            UnitPrice = d.UnitPrice
        }).ToList()
    };

    _context.PurchaseOrders.Add(entity);
    await _context.SaveChangesAsync(ct);

    return new PurchaseOrderDto(
        entity.PurchaseOrderId,
        entity.OrderDate,
        entity.SupplierId,
        entity.Status
    );
}

    public async Task<bool> UpdateAsync(int id, UpdatePurchaseOrderDto dto, CancellationToken ct = default)
    {
        var entity = await _context.PurchaseOrders.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;

        entity.OrderDate = dto.OrderDate;
        entity.SupplierId = dto.SupplierId;
        entity.Status = dto.Status;

        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _context.PurchaseOrders.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;

        _context.PurchaseOrders.Remove(entity);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
