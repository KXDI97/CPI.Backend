using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SupplierService.Application.Supplier;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync(CancellationToken ct = default);
    Task<SupplierDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<SupplierDto> CreateAsync(CreateSupplierDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdateSupplierDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
