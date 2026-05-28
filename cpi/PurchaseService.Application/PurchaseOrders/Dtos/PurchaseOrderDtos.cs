namespace PurchaseService.Application.PurchaseOrders.Dtos;

public record PurchaseOrderDto(
    int PurchaseOrderId, DateTime OrderDate, int SupplierId, string Status
);

public record CreatePurchaseOrderDto(
    int SupplierId,
    DateTime OrderDate,
    List<CreatePurchaseOrderDetailDto> Details
);


public record UpdatePurchaseOrderDto(
    DateTime OrderDate, int SupplierId, string Status
);

public record DeletePurchaseOrderDto(
    int PurchaseOrderId
);