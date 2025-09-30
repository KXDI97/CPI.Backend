namespace PurchaseOrderService.Domain.Entities;

public class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public required int SupplierId { get; set; }
    public string Status { get; set; } = "Pending"; // Pending | Approved | Rejected
    public LogicalCost? LogicalCost { get; set; }
    public ICollection<PurchaseOrderDetail> Details { get; set; } = new List<PurchaseOrderDetail>();
}