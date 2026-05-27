using System.Threading;
using PurchaseService.Application.PurchaseOrders.Dtos;

namespace PurchaseService.Application.PurchaseOrders;

public interface IPurchaseOrderDetailService
{
    Task<IEnumerable<PurchaseOrderDetailDto>> GetAllByOrderIdAsync(int purchaseOrderId, CancellationToken ct = default);
    Task<PurchaseOrderDetailDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PurchaseOrderDetailDto> CreateAsync(CreatePurchaseOrderDetailDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdatePurchaseOrderDetailDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
