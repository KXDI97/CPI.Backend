using Microsoft.EntityFrameworkCore;
using PurchaseOrderService.Application.Purchase;
using PurchaseOrderService.Domain.Entities;
using PurchaseOrderService.Infrastructure.Data;

namespace PurchaseOrderService.Infrastructure.Purchase;

public class PurchaseOrderDetailService : IPurchaseOrderDetailService
{
    private readonly CpiDbContext _context;

    public PurchaseOrderDetailService(CpiDbContext context) => _context = context;

    public async Task<IEnumerable<PurchaseOrderDetailDto>> GetAllByOrderIdAsync(int purchaseOrderId, CancellationToken ct = default)
        => await _context.PurchaseOrderDetails
            .Where(d => d.PurchaseOrderId == purchaseOrderId)
            .Select(d => new PurchaseOrderDetailDto(
                d.PurchaseOrderDetailId, d.PurchaseOrderId, d.ProductId,
                (int)d.Quantity, d.UnitPrice, d.LineTotal
            ))
            .ToListAsync(ct);

    public async Task<PurchaseOrderDetailDto?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.PurchaseOrderDetails
            .Where(d => d.PurchaseOrderDetailId == id)
            .Select(d => new PurchaseOrderDetailDto(
                d.PurchaseOrderDetailId, d.PurchaseOrderId, d.ProductId,
                (int)d.Quantity, d.UnitPrice, d.LineTotal
            ))
            .FirstOrDefaultAsync(ct);

    public async Task<PurchaseOrderDetailDto> CreateAsync(CreatePurchaseOrderDetailDto dto, CancellationToken ct = default)
    {
        var entity = new PurchaseOrderDetail
        {
            PurchaseOrderId = dto.PurchaseOrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice
        };
        _context.PurchaseOrderDetails.Add(entity);
        await _context.SaveChangesAsync(ct);
        return new PurchaseOrderDetailDto(entity.PurchaseOrderDetailId, entity.PurchaseOrderId, entity.ProductId,
            (int)entity.Quantity, entity.UnitPrice, entity.LineTotal);
    }

    public async Task<bool> UpdateAsync(int id, UpdatePurchaseOrderDetailDto dto, CancellationToken ct = default)
    {
        var entity = await _context.PurchaseOrderDetails.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;

        entity.ProductId = dto.ProductId;
        entity.Quantity = dto.Quantity;
        entity.UnitPrice = dto.UnitPrice;

        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _context.PurchaseOrderDetails.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;

        _context.PurchaseOrderDetails.Remove(entity);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
