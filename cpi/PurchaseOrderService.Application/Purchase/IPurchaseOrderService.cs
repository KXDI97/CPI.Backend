using System.Threading;

namespace PurchaseOrderService.Application.Purchase;
public interface IPurchaseOrderService
{
    Task<IEnumerable<PurchaseOrderDto>> GetAllAsync(CancellationToken ct = default);
    Task<PurchaseOrderDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdatePurchaseOrderDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
