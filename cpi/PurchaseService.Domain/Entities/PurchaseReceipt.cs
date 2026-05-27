namespace PurchaseService.Domain.Entities;

public class PurchaseReceipt
{
    public int ReceiptId { get; set; }
    public int PurchaseOrderId { get; set; }
    public DateTime ReceiptDate { get; set; }
    public ICollection<PurchaseReceiptDetail> Details { get; set; } = new List<PurchaseReceiptDetail>();

    public PurchaseOrder? PurchaseOrder { get; set; }

}
