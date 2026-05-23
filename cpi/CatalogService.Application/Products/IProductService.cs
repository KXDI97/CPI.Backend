namespace CatalogService.Application.Products;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ProductDto>> GetBySupplierAsync(int supplierId, CancellationToken ct = default);
    Task<ProductDto?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(string id, UpdateProductDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(string id, CancellationToken ct = default);
}