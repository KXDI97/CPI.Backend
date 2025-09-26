namespace PurchaseReceiptService.Domain.Entities;

public class PurchaseReceiptDetail
{
    public int    ReceiptDetailId         { get; set; }
    public int    ReceiptId               { get; set; }
    public int    ProductId               { get; set; }
    public int    QuantityReceived        { get; set; }
    public decimal UnitCost               { get; set; }
}