using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PurchaseReceiptService.Application.Purchase;

public record PurchaseReceiptDetailDto(
    int ReceiptDetailId,
    int ReceiptId,
    string ProductId,
    decimal QuantityReceived,
    decimal UnitCost
);

public record CreatePurchaseReceiptDetailDto(
    int ReceiptId,
    string ProductId,
    decimal QuantityReceived,
    decimal UnitCost
);

public record UpdatePurchaseReceiptDetailDto(
    string ProductId,
    decimal QuantityReceived,
    decimal UnitCost
);
