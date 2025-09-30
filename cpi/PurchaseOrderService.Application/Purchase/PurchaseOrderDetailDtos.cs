namespace PurchaseOrderService.Application.Purchase;

public class PurchaseOrderDetailDto
{
    public int PurchaseOrderDetailId { get; set; }
    public int PurchaseOrderId { get; set; }
    public string ProductId { get; set; }= string.Empty;
   public decimal Quantity { get; set; }
     public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; init; }

}
public class CreatePurchaseOrderDetailDto
{
    public int PurchaseOrderId { get; set; }
    public string ProductId { get; set; } = string.Empty; // inicializado
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
public record UpdatePurchaseOrderDetailDto(
    int    PurchaseOrderId, string    ProductId,
    decimal    Quantity, decimal UnitPrice, decimal LineTotal
);

public record DeletePurchaseOderDetailDto(
    int PurchaseOrderDetailId
);
