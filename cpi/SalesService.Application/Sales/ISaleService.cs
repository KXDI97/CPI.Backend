namespace SalesService.Application.Sales;

public interface ISaleService
{
    Task<IEnumerable<SaleSummaryDto>> GetAllAsync(CancellationToken ct = default);
    Task<SaleDto?>                    GetByIdAsync(int id, CancellationToken ct = default);
    Task<SaleDto>                     CreateAsync(CreateSaleDto dto, CancellationToken ct = default);
    Task<bool>                        UpdateStatusAsync(int id, UpdateSaleStatusDto dto, CancellationToken ct = default);
    Task<bool>                        DeleteAsync(int id, CancellationToken ct = default);
}