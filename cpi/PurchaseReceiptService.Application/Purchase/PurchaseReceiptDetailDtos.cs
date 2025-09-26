using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PurchaseReceiptService.Application.Purchase;

public record PurchaseReceiptDetailDto(
    int ReceiptDetailId,
    int ReceiptId,
    int ProductId,
    int QuantityReceived,
    decimal UnitCost
);

public record CreatePurchaseReceiptDetailDto(
    int ReceiptId,
    int ProductId,
    int QuantityReceived,
    decimal UnitCost
);

public record UpdatePurchaseReceiptDetailDto(
    int ProductId,
    int QuantityReceived,
    decimal UnitCost
);
