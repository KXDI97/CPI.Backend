using CPI.Application.Purchase.Dtos;


namespace CPI.SharedKernel.Interfaces.Purchase
{
    public interface ILogicalCostService
    {
        Task<LogicalCostDto> CreateAsync(CreateLogicalCostDto dto);
        Task<LogicalCostDto?> GetByOrderNumberAsync(int orderNumber);
        Task<LogicalCostDto> UpdateAsync(UpdateLogicalCostDto dto);
        Task<bool> DeleteAsync(int orderNumber);
    }
}
