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

public class PurchaseReceiptService : IPurchaseReceiptService
{
    private readonly CpiDbContext _db;

    public PurchaseReceiptService(CpiDbContext db) => _db = db;

    public async Task<IEnumerable<PurchaseReceiptDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.PurchaseReceipts
            .Select(r => new PurchaseReceiptDto(r.ReceiptId, r.PurchaseOrderId, r.ReceiptDate))
            .ToListAsync(ct);
    }

    public async Task<PurchaseReceiptDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var r = await _db.PurchaseReceipts.FirstOrDefaultAsync(x => x.ReceiptId == id, ct);
        return r is null ? null : new PurchaseReceiptDto(r.ReceiptId, r.PurchaseOrderId, r.ReceiptDate);
    }

    public async Task<PurchaseReceiptDto> CreateAsync(CreatePurchaseReceiptDto dto, CancellationToken ct = default)
    {
        var entity = new PurchaseReceipt
        {
            PurchaseOrderId = dto.PurchaseOrderId,
            ReceiptDate = dto.ReceiptDate
        };

        _db.PurchaseReceipts.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new PurchaseReceiptDto(entity.ReceiptId, entity.PurchaseOrderId, entity.ReceiptDate);
    }

    public async Task<bool> UpdateAsync(int id, UpdatePurchaseReceiptDto dto, CancellationToken ct = default)
    {
        var entity = await _db.PurchaseReceipts.FirstOrDefaultAsync(x => x.ReceiptId == id, ct);
        if (entity is null) return false;

        entity.PurchaseOrderId = dto.PurchaseOrderId;
        entity.ReceiptDate = dto.ReceiptDate;

        await _db.SaveChangesAsync(ct);
        return true;
    }
}
