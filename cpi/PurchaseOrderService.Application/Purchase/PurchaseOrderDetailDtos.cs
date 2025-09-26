namespace PurchaseOrderService.Application.Purchase;

public record PurchaseOrderDetailDto(
    int    PurchaseOrderDetailId, int    PurchaseOrderId, string    ProductId,
    int    Quantity, decimal UnitPrice, decimal LineTotal
);

public record CreatePurchaseOrderDetailDto(
    int    PurchaseOrderId, string    ProductId,
    int    Quantity, decimal UnitPrice, decimal LineTotal
);

public record UpdatePurchaseOrderDetailDto(
    int    PurchaseOrderId, string    ProductId,
    int    Quantity, decimal UnitPrice, decimal LineTotal
);

public record DeletePurchaseOderDetailDto(
    int PurchaseOrderDetailId
);