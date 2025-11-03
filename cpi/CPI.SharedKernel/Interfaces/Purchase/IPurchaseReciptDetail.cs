using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CPI.Application.Purchase.Dtos;


namespace CPI.SharedKernel.Interfaces.Purchase;

public interface IPurchaseReceiptDetailService
{
    Task<IEnumerable<PurchaseReceiptDetailDto>> GetAllByReceiptIdAsync(int receiptId, CancellationToken ct = default);
    Task<PurchaseReceiptDetailDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PurchaseReceiptDetailDto> CreateAsync(CreatePurchaseReceiptDetailDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdatePurchaseReceiptDetailDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
