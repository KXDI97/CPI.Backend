namespace PurchaseOrderService.Application.Purchase;

public record PurchaseOrderDto(
    int PurchaseOrderId, DateTime OrderDate, int SupplierId, string Status
);
public record CreatePurchaseOrderDto(
    DateTime OrderDate, int SupplierId
);
public record UpdatePurchaseOrderDto(
    DateTime OrderDate, int SupplierId, string Status
);
public record DeletePurchaseOrderDto(
    int PurchaseOrderId
);