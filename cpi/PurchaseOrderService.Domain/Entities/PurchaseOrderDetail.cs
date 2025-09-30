namespace PurchaseOrderService.Domain.Entities;

public class PurchaseOrderDetail
{
    public int PurchaseOrderDetailId { get; set; }
    public int PurchaseOrderId { get; set; }
    public  PurchaseOrder? PurchaseOrder { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; private set; }

}    