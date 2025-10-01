namespace PurchaseReceiptService.Domain.Entities;

public class PurchaseReceiptDetail
{
    public int    ReceiptDetailId         { get; set; }
    public int    ReceiptId               { get; set; }
    public string    ProductId               { get; set; }
    public decimal    QuantityReceived        { get; set; }
    public decimal UnitCost               { get; set; }
}