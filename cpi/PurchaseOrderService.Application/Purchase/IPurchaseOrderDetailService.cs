using System.Threading;

namespace PurchaseOrderService.Application.Purchase;

public interface IPurchaseOrderDetailService
{
    Task<IEnumerable<PurchaseOrderDetailDto>> GetAllByOrderIdAsync(int purchaseOrderId, CancellationToken ct = default);
    Task<PurchaseOrderDetailDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PurchaseOrderDetailDto> CreateAsync(CreatePurchaseOrderDetailDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdatePurchaseOrderDetailDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
