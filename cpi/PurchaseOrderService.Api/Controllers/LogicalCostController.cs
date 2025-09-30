using Microsoft.AspNetCore.Mvc;
using PurchaseOrderService.Application.Purchase;

namespace PurchaseOrderService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogicalCostsController : ControllerBase
    {
        private readonly ILogicalCostService _logicalCostService;

        public LogicalCostsController(ILogicalCostService logicalCostService)
        {
            _logicalCostService = logicalCostService;
        }

        /// <summary>
        /// Crear costos lógicos para una orden de compra
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<LogicalCostDto>> Create(CreateLogicalCostDto dto)
        {
            var result = await _logicalCostService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByOrderNumber), new { orderNumber = result.OrderNumber }, result);
        }

        /// <summary>
        /// Obtener costos lógicos por número de orden
        /// </summary>
        [HttpGet("{orderNumber:int}")]
        public async Task<ActionResult<LogicalCostDto>> GetByOrderNumber(int orderNumber)
        {
            var result = await _logicalCostService.GetByOrderNumberAsync(orderNumber);
            if (result == null)
                return NotFound($"No se encontraron costos lógicos para la orden {orderNumber}");

            return Ok(result);
        }

        /// <summary>
        /// Actualizar costos lógicos de una orden
        /// </summary>
        [HttpPut("{orderNumber:int}")]
        public async Task<ActionResult<LogicalCostDto>> Update(int orderNumber, UpdateLogicalCostDto dto)
        {
            if (orderNumber != dto.OrderNumber)
                return BadRequest("El número de orden no coincide con el DTO.");

            var result = await _logicalCostService.UpdateAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Eliminar costos lógicos de una orden
        /// </summary>
        [HttpDelete("{orderNumber:int}")]
        public async Task<ActionResult> Delete(int orderNumber)
        {
            var deleted = await _logicalCostService.DeleteAsync(orderNumber);
            if (!deleted)
                return NotFound($"No se encontraron costos lógicos para eliminar en la orden {orderNumber}");

            return NoContent();
        }
    }
}
