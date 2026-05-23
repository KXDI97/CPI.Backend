namespace InventoryService.Domain.Entities;

public class InventoryItem
{
    public string ProductId { get; set; } = default!;
    public int WarehouseId { get; set; }
    public decimal OnHand { get; set; }   // decimal(18,4)
    public decimal Reserved { get; set; }

    // PK compuesta ProductId + WarehouseId
}
