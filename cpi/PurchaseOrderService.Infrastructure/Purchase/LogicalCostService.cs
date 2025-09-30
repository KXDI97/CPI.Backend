using Microsoft.EntityFrameworkCore;
using PurchaseOrderService.Application.Purchase;
using PurchaseOrderService.Domain.Entities;
using PurchaseOrderService.Infrastructure.Data;

namespace PurchaseOrderService.Infrastructure.Purchase
{
    public class LogicalCostService : ILogicalCostService
    {
        private readonly CpiDbContext _context;

        public LogicalCostService(CpiDbContext context)
        {
            _context = context;
        }

        public async Task<LogicalCostDto> CreateAsync(CreateLogicalCostDto dto)
        {
            var exists = await _context.LogicalCosts
                .AnyAsync(x => x.OrderNumber == dto.OrderNumber);

            if (exists)
                throw new InvalidOperationException(
                    $"Ya existen costos lógicos para la orden {dto.OrderNumber}");

            var entity = new LogicalCost
            {
                OrderNumber = dto.OrderNumber,
                InternationalTransport = dto.InternationalTransport,
                LocalTransport = dto.LocalTransport,
                Nationalization = dto.Nationalization,
                CargoInsurance = dto.CargoInsurance,
                Storage = dto.Storage,
                Others = dto.Others
            };

            _context.LogicalCosts.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<LogicalCostDto?> GetByOrderNumberAsync(int orderNumber)
        {
            var entity = await _context.LogicalCosts
                .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);

            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<LogicalCostDto> UpdateAsync(UpdateLogicalCostDto dto)
        {
            var entity = await _context.LogicalCosts
                .FirstOrDefaultAsync(x => x.OrderNumber == dto.OrderNumber);

            if (entity == null)
                throw new KeyNotFoundException("LogicalCost not found");

            entity.InternationalTransport = dto.InternationalTransport;
            entity.LocalTransport = dto.LocalTransport;
            entity.Nationalization = dto.Nationalization;
            entity.CargoInsurance = dto.CargoInsurance;
            entity.Storage = dto.Storage;
            entity.Others = dto.Others;

            await _context.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(int orderNumber)
        {
            var entity = await _context.LogicalCosts
                .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);

            if (entity == null) return false;

            _context.LogicalCosts.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static LogicalCostDto MapToDto(LogicalCost entity) =>
            new()
            {
                OrderNumber = entity.OrderNumber,
                InternationalTransport = entity.InternationalTransport,
                LocalTransport = entity.LocalTransport,
                Nationalization = entity.Nationalization,
                CargoInsurance = entity.CargoInsurance,
                Storage = entity.Storage,
                Others = entity.Others
            };
    }
}
