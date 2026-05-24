namespace CatalogService.Application.Suppliers;
 
public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync(CancellationToken ct = default);
    Task<SupplierDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<SupplierDto> CreateAsync(CreateSupplierDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdateSupplierDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}