namespace InventoryService.Domain.Entities;

public class InventoryMovement
{
    public long MovementId { get; set; }    // identity
    public DateTime At { get; set; } = DateTime.UtcNow;
    public string ProductId { get; set; } = default!;
    public int WarehouseId { get; set; }
    public decimal Qty { get; set; }        // +entrada / -salida
    public string Reason { get; set; } = "Adjustment"; // Purchase/Sale/Return/Adjustment
}
