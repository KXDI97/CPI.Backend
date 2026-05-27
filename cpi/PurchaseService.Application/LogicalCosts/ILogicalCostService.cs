using System.Threading.Tasks;
using PurchaseService.Application.LogicalCosts.Dtos;


namespace PurchaseService.Application.LogicalCosts
{
    public interface ILogicalCostService
    {
        Task<LogicalCostDto> CreateAsync(CreateLogicalCostDto dto);
        Task<LogicalCostDto?> GetByOrderNumberAsync(int orderNumber);
        Task<LogicalCostDto> UpdateAsync(UpdateLogicalCostDto dto);
        Task<bool> DeleteAsync(int orderNumber);
    }
}
