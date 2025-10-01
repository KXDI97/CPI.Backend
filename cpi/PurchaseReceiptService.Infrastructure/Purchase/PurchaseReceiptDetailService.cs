using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using PurchaseReceiptService.Application.Purchase;  
using PurchaseReceiptService.Domain.Entities;       
using PurchaseReceiptService.Infrastructure.Data;   


namespace PurchaseReceiptService.Infrastructure.Purchase;

public class PurchaseReceiptDetailService : IPurchaseReceiptDetailService
{
    private readonly CpiDbContext _db;

    public PurchaseReceiptDetailService(CpiDbContext db) => _db = db;

    public async Task<IEnumerable<PurchaseReceiptDetailDto>> GetAllByReceiptIdAsync(int receiptId, CancellationToken ct = default)
    {
        return await _db.PurchaseReceiptDetails
            .Where(d => d.ReceiptId == receiptId)
            .Select(d => new PurchaseReceiptDetailDto(d.ReceiptDetailId, d.ReceiptId, d.ProductId, d.QuantityReceived, d.UnitCost))
            .ToListAsync(ct);
    }

    public async Task<PurchaseReceiptDetailDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var d = await _db.PurchaseReceiptDetails.FirstOrDefaultAsync(x => x.ReceiptDetailId == id, ct);
        return d is null ? null : new PurchaseReceiptDetailDto(d.ReceiptDetailId, d.ReceiptId, d.ProductId, d.QuantityReceived, d.UnitCost);
    }

    public async Task<PurchaseReceiptDetailDto> CreateAsync(CreatePurchaseReceiptDetailDto dto, CancellationToken ct = default)
{
    var entity = new PurchaseReceiptDetail
    {
        ReceiptId = dto.ReceiptId,
        ProductId = dto.ProductId,
        QuantityReceived = dto.QuantityReceived,
        UnitCost = dto.UnitCost
    };

    _db.PurchaseReceiptDetails.Add(entity);
    await _db.SaveChangesAsync(ct);

    // Recuperamos el último ID insertado para ese ReceiptId y ProductId
    var insertedId = await _db.PurchaseReceiptDetails
        .Where(d => d.ReceiptId == dto.ReceiptId && d.ProductId == dto.ProductId)
        .OrderByDescending(d => d.ReceiptDetailId)
        .Select(d => d.ReceiptDetailId)
        .FirstOrDefaultAsync(ct);

    return new PurchaseReceiptDetailDto(insertedId, entity.ReceiptId, entity.ProductId, entity.QuantityReceived, entity.UnitCost);
}

    public async Task<bool> UpdateAsync(int id, UpdatePurchaseReceiptDetailDto dto, CancellationToken ct = default)
    {
        var entity = await _db.PurchaseReceiptDetails.FirstOrDefaultAsync(x => x.ReceiptDetailId == id, ct);
        if (entity is null) return false;

        entity.ProductId = dto.ProductId;
        entity.QuantityReceived = dto.QuantityReceived;
        entity.UnitCost = dto.UnitCost;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.PurchaseReceiptDetails.FirstOrDefaultAsync(x => x.ReceiptDetailId == id, ct);
        if (entity is null) return false;

        _db.PurchaseReceiptDetails.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
