namespace PurchaseService.Domain.Entities;


public class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public required int SupplierId { get; set; }
        // Navegación hijos
    public ICollection<PurchaseOrderDetail> Details { get; set; } = new List<PurchaseOrderDetail>();
    
    // Estas dos son las que el DbContext busca — deben llamarse EXACTAMENTE así
    public PurchaseReceipt? Receipt { get; set; }
    public Transaction? Transaction { get; set; }
    public string Status { get; set; } = "Pending"; // Pending | Approved | Rejected
    public LogicalCost? LogicalCost { get; set; }

}