namespace PurchaseService.Domain.Entities;

public class PurchaseReceiptDetail
{
    public int    ReceiptDetailId         { get; set; }
    public int    ReceiptId               { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public decimal    QuantityReceived        { get; set; }
    public decimal UnitCost               { get; set; }

    public PurchaseReceipt? Receipt { get; set; }
}