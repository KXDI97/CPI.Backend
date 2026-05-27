using System.Threading;
using PurchaseService.Application.PurchaseOrders.Dtos;

namespace PurchaseService.Application.PurchaseOrders;
public interface IPurchaseOrderService
{
    Task<IEnumerable<PurchaseOrderDto>> GetAllAsync(CancellationToken ct = default);
    Task<PurchaseOrderDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdatePurchaseOrderDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
