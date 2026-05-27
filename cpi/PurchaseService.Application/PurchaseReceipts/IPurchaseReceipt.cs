using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PurchaseService.Application.PurchaseReceipts.Dtos;

namespace PurchaseService.Application.PurchaseReceipts;

public interface IPurchaseReceiptService
{
    Task<IEnumerable<PurchaseReceiptDto>> GetAllAsync(CancellationToken ct = default);
    Task<PurchaseReceiptDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PurchaseReceiptDto> CreateAsync(CreatePurchaseReceiptDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdatePurchaseReceiptDto dto, CancellationToken ct = default);

}
